using openglclevel_server_security.TokenManager;
using System.IdentityModel.Tokens.Jwt;

namespace openglclevel_server_api.UtilControllers
{
    public class ControllerUtilities
    {
        private readonly TokenHandlerEngine _tokenHandler;
        public ControllerUtilities(TokenHandlerEngine tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public  static string GetTokenFromContextRequest(HttpContext context)
        {
            var tokenHeader = context.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();

            var token = bearerToken?.Split("Bearer ")[1];
            return token;
        }

        public static Guid GetUserIdFromRequestContext(HttpContext context)
        {

            string bearerToken = GetTokenFromContextRequest(context);
            var tokenDecoded = TokenHandlerEngine.GetTokenDataByStringValue(bearerToken);

            var userID = tokenDecoded.Claims.Where(W => W.Type == "userID").FirstOrDefault().Value;
            return Guid.Parse(userID.ToUpper());
        }


    }
}
