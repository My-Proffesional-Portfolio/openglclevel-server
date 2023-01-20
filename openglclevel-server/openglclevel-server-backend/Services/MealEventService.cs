using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.Requests.MealEventItems;
using openglclevel_server_models.Requests.MealEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_backend.Services
{
    public class MealEventService : IMealEventService
    {
        private readonly IMealItemService _mealItemSC;
        private readonly IMealEventItemsRepository _eventItemRepo;
        private readonly IMealEventRepository _eventRepo;
        private readonly OpenglclevelContext _dbContext;
        public MealEventService(IMealItemService mealItemSC, IMealEventItemsRepository eventItemRepo, 
            IMealEventRepository eventRepo, OpenglclevelContext dbContext )
        {
            _mealItemSC = mealItemSC;
            _eventItemRepo = eventItemRepo;
            _eventRepo = eventRepo;
            _dbContext = dbContext;
        }

        public async Task<Guid> AddMealEvent(Guid userID, NewMealEventModel mealEvent)
        {
            List<ExistingMealItemPair> normalizedInputItems = await AddNewMealItemsForEvent(userID, mealEvent);

            mealEvent.ItemMeals = mealEvent.ItemMeals.Union(normalizedInputItems).ToList();

            var newMealEventDB = new MealEvent();
            newMealEventDB.MealAtDay = mealEvent.MealType.ToString();
            newMealEventDB.MealDate = mealEvent.EventDate;
            newMealEventDB.CreationDate = DateTime.Now;
            newMealEventDB.GlcLevel = mealEvent.GlcLevel;
            newMealEventDB.Notes = mealEvent.Postprandial ? "Posprandial" : "";
            newMealEventDB.Id = Guid.NewGuid();
            newMealEventDB.UserId = userID;

            var mealEventItems = new List<MealEventItem>();

            mealEvent.ItemMeals.ForEach(fe =>
            {
                mealEventItems.Add(new MealEventItem()
                {
                    Id = Guid.NewGuid(),
                    MealEventId = newMealEventDB.Id, 
                    Description = "", 
                    Unit = fe.Quantity,
                    MealItemId = fe.ID
                });
            });

            await _eventItemRepo.AddRangeAsync(mealEventItems);
            await _eventRepo.AddAsync(newMealEventDB);

            await _dbContext.SaveChangesAsync();
            return newMealEventDB.Id;

        }

        private async Task<List<ExistingMealItemPair>> AddNewMealItemsForEvent(Guid userID, NewMealEventModel mealEvent)
        {
            var normalizedInputItems = new List<ExistingMealItemPair>();
            if (mealEvent.NewMeals.Count > 0)
            {

                foreach (var nM in mealEvent.NewMeals)
                {
                    var localMeal = new NewMealItemModel();
                    localMeal.Name = nM.Name;
                    var mealDB = await _mealItemSC.AddSingleMealItemToUser(userID, localMeal);

                    normalizedInputItems.Add(new ExistingMealItemPair() { ID = mealDB.ID, Quantity = nM.Quantity });
                }
            }

            return normalizedInputItems;
        }
    }
}
