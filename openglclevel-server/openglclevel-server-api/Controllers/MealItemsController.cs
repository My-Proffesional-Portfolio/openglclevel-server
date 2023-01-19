using Microsoft.AspNetCore.Mvc;
using openglclevel_server_models.Requests.MealEventItems;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace openglclevel_server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealItemsController : ControllerBase
    {
        // GET: api/<MealItemsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MealItemsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MealItemsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<NewMealItems> meals)
        {
        }

        // PUT api/<MealItemsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MealItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
