using openglclevel_server_models.Pagination;
using openglclevel_server_models.Requests.MealEvents;
using openglclevel_server_models.Responses;
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
        public Task<PaginationListEntityModel<MealEventModel>> GetEvents(int page, int itemsPerPage, string searchTerm = null);
        public Task<UserMetricsModel> GetEventsGlcAverage();
       MealEventDedtailsModel GetMealEventDetails(Guid eventId);
    }
}
