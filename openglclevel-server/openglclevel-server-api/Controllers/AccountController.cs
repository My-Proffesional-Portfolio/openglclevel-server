using Microsoft.AspNetCore.Mvc;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_models.API.Security;
using openglclevel_server_models.Requests.Accounts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace openglclevel_server_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IUserService _userSC;
        private readonly ISecurityKeys _securityKeysValues;

        public AccountController(IUserService userSC, ISecurityKeys securityKeys)
        {
            _userSC = userSC;
            _securityKeysValues = securityKeys;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Post([FromBody] NewRegisterModel value)
        {
           var result = await _userSC.RegisterUser(value);
           return Ok(result);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
