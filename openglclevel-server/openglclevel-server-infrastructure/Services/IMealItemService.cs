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
    }
}
