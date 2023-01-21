using Microsoft.AspNetCore.Http;
using openglclevel_server_data.DataAccess;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.Pagination;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MealItemService(IMealItemRepository mealItemRepository, OpenglclevelContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _mealItemRepository = mealItemRepository;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
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
    
        public async Task<object> GetMealItems(Guid userID, int page, int itemsPerPage, string searchTerm = null)
        {
            var mealItems = _mealItemRepository.FindByExpresion(w => w.UserId == userID);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                mealItems = mealItems.Where(w => w.MealName.Contains(searchTerm));
            }

            var data = await _mealItemRepository.GetAllPagedAsync(page, itemsPerPage,
                sorter: (o => o.MealName), mealItems);

            var response = new PaginationListEntityModel<NewMealItemModelDB>();

            response.TotalPages = data.TotalPages;
            response.TotalCount = data.TotalCount; 
            response.PageNumber = data.PageNumber;
            response.PagedList = new List<NewMealItemModelDB>();

            response.PagedList = data.PagedList.Select(s => new NewMealItemModelDB
            {
                Name = s.MealName, 
                ID = s.Id

            }).ToList();

            return response;
        }
    
    }
}
