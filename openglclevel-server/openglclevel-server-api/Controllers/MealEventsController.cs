using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using openglclevel_server_api.Filters;
using openglclevel_server_api.UtilControllers;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.API;
using openglclevel_server_models.Requests.MealEventItems;
using openglclevel_server_models.Requests.MealEvents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace openglclevel_server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealEventsController : ControllerBase
    {
        // GET: api/<MealEventsController>
        private readonly IMealEventService _eventSC;
        private readonly ControllerUtilities _utilities;
        public MealEventsController(IMealEventService eventSC, ControllerUtilities utilities)
        {
            _eventSC = eventSC;
            _utilities = utilities;
        }
        [HttpGet]
        [AutomaticExceptionHandler]
        [UserActionFilter]
        public async Task<IActionResult> Get(int page, int itemsPerPage, string searchTerm = "")
        {
            var result = await _eventSC.GetEvents(page, itemsPerPage, searchTerm);
            return Ok(result);
        }

        [HttpGet]
        [Route("getEventMealTypes")]
        public IActionResult GetEventMealTypes()
        {
            var result =  MealTypes.GetMealTypesDefinition();
            return Ok(result);
        }

        [HttpGet]
        [Route("userEventMetrics")]
        [AutomaticExceptionHandler]
        [UserActionFilter]
        public async Task<IActionResult> GetEventsGlcAverage()
        {
            var result = await _eventSC.GetEventsGlcAverage();
            return Ok(result);
        }

        // POST api/<MealEventsController>
        [HttpPost]
        [AutomaticExceptionHandler]
        [UserActionFilter]
        public async Task<IActionResult> Post([FromBody] NewMealEventModel newEvent)
        {
            var result = await _eventSC.AddMealEvent(newEvent);
            return Ok(result);
        }


    }
}
