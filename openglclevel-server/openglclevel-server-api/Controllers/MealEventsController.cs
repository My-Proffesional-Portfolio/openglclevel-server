using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using openglclevel_server_api.UtilControllers;
using openglclevel_server_infrastructure.Services;
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
        public async Task<IActionResult> Get(int page, int itemsPerPage, string searchTerm = "")
        {
            var userID = _utilities.GetUserIdFromRequestContext(HttpContext);
            var result = await _eventSC.GetEvents(userID, page, itemsPerPage, searchTerm);
            return Ok(result);
        }

        // POST api/<MealEventsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewMealEventModel newEvent)
        {
            var userID = _utilities.GetUserIdFromRequestContext(HttpContext);
            var result = await _eventSC.AddMealEvent(userID, newEvent);
            return Ok(result);
        }


    }
}
