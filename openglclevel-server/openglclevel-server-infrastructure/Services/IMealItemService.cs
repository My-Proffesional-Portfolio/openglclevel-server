using openglclevel_server_models.Requests.MealEventItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Services
{
    public interface IMealItemService
    {
        public Task<List<NewMealItemModel>> AddMealItemsFromUserID(Guid userID, List<NewMealItemModel> meals);
        public Task<NewMealItemModelDB> AddSingleMealItemToUser(Guid userID, NewMealItemModel meal);
        public Task<object> GetMealItems(Guid userID, int page, int itemsPerPage, string searchTerm = null);

    }
}
