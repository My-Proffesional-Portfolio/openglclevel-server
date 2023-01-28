using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using openglclevel_server_api.Filters;
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
        private readonly IHttpContextAccessor _httpContext;

        public AccountController(IUserService userSC, ISecurityKeys securityKeys, IHttpContextAccessor httpContext)
        {
            _userSC = userSC;
            _securityKeysValues = securityKeys;
            _httpContext = httpContext;
        }

        [HttpGet]
        [Route("login")]
        [AutomaticExceptionHandler]
        public async Task<IActionResult> login(string userName, string password)
        {
            var result = await _userSC.Login(userName, password);
            _httpContext.HttpContext.Session.SetString("userID", result.UserID.ToString());
            return Ok(result);
        }

        [HttpGet]
        [Route("userData")]
        [Authorize]
        public IActionResult userData(string userName)
        {
            var result = new {userName = "userName"};
            return Ok(result);
        }


        // POST api/<AccountController>
        [HttpPost]
        [Route("register")]
        [AutomaticExceptionHandler]
        public async Task<IActionResult> Post([FromBody] NewRegisterModel value)
        {
           var result = await _userSC.RegisterUser(value);
           return Ok(result);
        }


    }
}
