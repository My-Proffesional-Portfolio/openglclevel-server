using openglclevel_server_models.Requests.MealEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglclevel_server_infrastructure.Services
{
    public interface IMealEventService
    {
        public Task<Guid> AddMealEvent(NewMealEventModel mealEvent);
        public Task<object> GetEvents(int page, int itemsPerPage, string searchTerm = null);
    }
}
