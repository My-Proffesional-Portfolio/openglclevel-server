﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using openglclevel_server_api.UtilControllers;
using openglclevel_server_backend.Services;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.Requests.MealEventItems;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace openglclevel_server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealItemsController : ControllerBase
    {
        private readonly ControllerUtilities _utilities;
        private readonly IMealItemService _mealItemSC;
        public MealItemsController(ControllerUtilities utilities, IMealItemService mealSC)
        {
            _utilities = utilities;
            _mealItemSC = mealSC;
        }

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
        [Route("addMealItems")]
        public async Task<IActionResult> Post([FromBody] List<NewMealItemModel> meals)
        {
            var userID =  _utilities.GetUserIdFromRequestContext(HttpContext);
            var items =  await _mealItemSC.AddMealItemsFromUserID(userID, meals);
            return Ok(items);
           
        }
    }
}
