using Microsoft.AspNetCore.Mvc;
using openglclevel_server_models.Requests.MealEventItems;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace openglclevel_server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealEventsController : ControllerBase
    {
        // GET: api/<MealEventsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MealEventsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MealEventsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


    }
}
