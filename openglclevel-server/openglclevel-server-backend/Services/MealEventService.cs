using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.Pagination;
using openglclevel_server_models.Requests.MealEventItems;
using openglclevel_server_models.Requests.MealEvents;
using openglclevel_server_models.Responses;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public MealEventService(IMealItemService mealItemSC, IMealEventItemsRepository eventItemRepo, 
            IMealEventRepository eventRepo, OpenglclevelContext dbContext, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            _mealItemSC = mealItemSC;
            _eventItemRepo = eventItemRepo;
            _eventRepo = eventRepo;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<Guid> AddMealEvent(NewMealEventModel mealEvent)
        {

            var userID = Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("userID"));
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

        public async Task<object> GetEvents(int page, int itemsPerPage, string searchTerm = null)
        {
            var userID = Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("userID"));
            var mealEvents = _eventRepo.FindByExpresion(w => w.UserId == userID);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                mealEvents = mealEvents.Where(w => w.Notes.Contains(searchTerm));
            }

            var data = await _eventRepo.GetAllPagedAsync(page, itemsPerPage,
                sorter: (o => o.CreationDate), mealEvents);

            var response = new PaginationListEntityModel<MealEventModel>();

            response.TotalPages = data.TotalPages;
            response.TotalCount = data.TotalCount;
            response.PageNumber = data.PageNumber;
            response.PagedList = new List<MealEventModel>();

            response.PagedList = data.PagedList.Select(s => new MealEventModel
            {
               Id = s.Id,
               EventDate = s.MealDate,
               GlcLevel = s.GlcLevel,

            }).ToList();

            return response;
        }

        public async Task<UserMetricsModel> GetEventsGlcAverage()
        {

            var result =  new UserMetricsModel();
            var userID = Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("userID"));
            var mealEvents = _eventRepo.FindByExpresion(w => w.UserId == userID);
            var average = mealEvents.Count() > 0 ? (decimal) await mealEvents.AverageAsync(a => a.GlcLevel) : 0m;

            result.GlcAverage = average;
            var lastEvent =  await mealEvents.OrderByDescending(o => o.CreationDate).FirstOrDefaultAsync();

            if (lastEvent != null)
                result.lastEventRegistered = lastEvent.MealDate;

            result.EventNumbers = await mealEvents.CountAsync();

            var user = await _userRepository.GetByIdAsync(userID);
            result.UserName = user.UserName;
            result.Name = user.Name + " " + user.FirstName;

            return result;

        }

    }
}
