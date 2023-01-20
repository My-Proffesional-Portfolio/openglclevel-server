using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.Requests.MealEventItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_backend.Services
{
    public class MealItemService : IMealItemService
    {
        private readonly IMealItemRepository _mealItemRepository;
        private readonly OpenglclevelContext _dbContext;
        public MealItemService(IMealItemRepository mealItemRepository, OpenglclevelContext dbContext)
        {
            _mealItemRepository = mealItemRepository;
            _dbContext = dbContext; 
        }
        public async Task<List<NewMealItemModel>> AddMealItemsFromUserID(Guid userID, List<NewMealItemModel> meals)
        {
            var mealItemsDB = new List<MealItem>();

            meals.ForEach(fe => mealItemsDB.Add(new MealItem
            {
                Id = Guid.NewGuid(),
                MealName = fe.Name,
                UserId = userID

            }));

            await _mealItemRepository.AddRangeAsync(mealItemsDB);
            _dbContext.SaveChanges();
            return meals;

        }

        public async Task<NewMealItemModelDB> AddSingleMealItemToUser(Guid userID, NewMealItemModel meal)
        {
            var mealItemsDB = new MealItem();

            var newMeal = new MealItem
            {
                Id = Guid.NewGuid(),
                MealName = meal.Name,
                UserId = userID

            };

            await _mealItemRepository.AddAsync(newMeal);
            _dbContext.SaveChanges();
            return new NewMealItemModelDB { ID = newMeal.Id, Name = newMeal.MealName};

        }
    }
}
