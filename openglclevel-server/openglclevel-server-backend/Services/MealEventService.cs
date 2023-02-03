using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models;
using openglclevel_server_models.API;
using openglclevel_server_models.Exceptions;
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
        private readonly IMealItemRepository _mealItemRepository;
        private readonly OpenglclevelContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public MealEventService(IMealItemService mealItemSC, IMealEventItemsRepository eventItemRepo,
            IMealEventRepository eventRepo, OpenglclevelContext dbContext, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository, IMealItemRepository mealItemRepository)
        {
            _mealItemSC = mealItemSC;
            _eventItemRepo = eventItemRepo;
            _eventRepo = eventRepo;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mealItemSC= mealItemSC;
            _mealItemRepository = mealItemRepository;
        }

        public async Task<Guid> AddMealEvent(NewMealEventModel mealEvent)
        {

            //var userID = Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("userID"));
            var userID = StaticMemoryVariables.UserID;
            List<ExistingMealItemPair> normalizedInputItems = await AddNewMealItemsForEvent(userID, mealEvent);

            mealEvent.ItemMeals = mealEvent.ItemMeals.Union(normalizedInputItems).ToList();

            var newMealEventDB = new MealEvent();
            newMealEventDB.MealAtDay = MealTypes.GetMealTypesDefinition().FirstOrDefault(f => f.Type == mealEvent.MealType).Name;
            newMealEventDB.MealDate = mealEvent.EventDate;
            newMealEventDB.CreationDate = DateTime.Now;
            newMealEventDB.GlcLevel = mealEvent.GlcLevel;
            newMealEventDB.Notes = mealEvent.Postprandial ? "Postprandial" : "";
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

        public async Task<PaginationListEntityModel<MealEventModel>> GetEvents(int page, int itemsPerPage, string searchTerm = null)
        {
            var userID = StaticMemoryVariables.UserID;
            var mealEvents = _eventRepo.FindByExpresion(w => w.UserId == userID);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                //mealEvents = mealEvents.Where(w => w.Notes.Contains(searchTerm));
                //mealEvents = mealEvents.SelectMany(sm=> sm.MealEventItems).Select(s=> s.MealItem).Where(w=> w.MealName == searchTerm).Select(s=> s.);
                //var meals = mealEvents.SelectMany(sm =>  sm.MealEventItems).Select(s => s.MealItem).Where(w => w.MealName.Contains(searchTerm)).Include(i=> i.MealEventItems);
                //var mealEventItems = meals.Select(s => s.MealEventItems);
                //var mealEventsQry =  mealEventItems.Select(s=> s.)
                var mealEventsWithMeal = _mealItemRepository.FindByExpresion(f => f.MealName.Contains(searchTerm)).Include(i=> i.MealEventItems)
                    .SelectMany(s => s.MealEventItems.AsEnumerable()).AsEnumerable();

                var mealEventIDs = mealEventsWithMeal.Select(s => s.MealEventId);

                 mealEvents = mealEvents.Where(w=> mealEventIDs.Contains(w.Id)).AsQueryable();
                //mealEvents = mealEvents.Where(w=> w.MealEventItems.Intersect(mealEventsWithMeal)

            }

            var data = await _eventRepo.GetAllPagedAsync(page, itemsPerPage,
                sorter: (o => o.MealDate), mealEvents, orderByDesc: true);

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
                Pospandrial = s.Notes == "Postprandial",

            }).ToList();

            return response;
        }

        public async Task<UserMetricsModel> GetEventsGlcAverage()
        {

            var result = new UserMetricsModel();

            try
            {
                //var userID = Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("userID"));
                var userID = StaticMemoryVariables.UserID;
                var mealEvents = _eventRepo.FindByExpresion(w => w.UserId == userID);
                var average = mealEvents.Count() > 0 ? (decimal)await mealEvents.AverageAsync(a => a.GlcLevel) : 0m;

                result.GlcAverage = average;
                var lastEvent = await mealEvents.OrderByDescending(o => o.CreationDate).FirstOrDefaultAsync();

                if (lastEvent != null)
                    result.lastEventRegistered = lastEvent.MealDate;

                result.EventNumbers = await mealEvents.CountAsync();

                var user = await _userRepository.GetByIdAsync(userID);
                result.UserName = user.UserName;
                result.Name = user.Name + " " + user.FirstName;
            }
            catch (Exception ex)
            {

                throw new FriendlyException("Error aqui: -> " + ex.Message);
            }


            return result;

        }

        public MealEventDedtailsModel GetMealEventDetails(Guid eventId)
        {
            var result = new MealEventDedtailsModel();
            var mealEvent = _eventRepo.FindByExpresion(f => f.Id == eventId).Include(i => i.MealEventItems).ThenInclude(thi => thi.MealItem).FirstOrDefault();

            result.GlcLevel = mealEvent.GlcLevel;
            result.Id = mealEvent.Id;
            result.EventDate = mealEvent.MealDate;
            result.Pospandrial = mealEvent.Notes == "Postprandial";
            result.MealTypeText = mealEvent.MealAtDay;
            result.MealList = mealEvent.MealEventItems
                .Select(s => new EventMealItemsModel()
                {
                    MealID = s.MealItem.Id,
                    MealName = s.MealItem.MealName,
                    Quantity = s.Unit,
                }).ToList();

            return result;
        }


        public async Task<bool> DeleteEventMealWithItems(Guid eventID)
        {
            var userID = StaticMemoryVariables.UserID;
            var eventToDelete = await _eventRepo.FindByExpresion(f=> f.UserId == userID && f.Id == eventID)
                .Include(i=> i.MealEventItems).FirstOrDefaultAsync();

            if (eventToDelete == null)
            {
                throw new FriendlyException("Event does not exist in database");
            }

            _eventItemRepo.DeleteRangeAsync(eventToDelete.MealEventItems);
            _eventRepo.DeleteAsync(eventToDelete);
            _dbContext.SaveChanges();

            return true;

        }
    }
}
