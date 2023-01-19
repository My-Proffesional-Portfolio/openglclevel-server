using openglclevel_server_security.TokenManager;

namespace openglclevel_server_api.UtilControllers
{
    public class ControllerUtilities
    {
        private readonly TokenHandlerEngine _tokenHandler;
        public ControllerUtilities(TokenHandlerEngine tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        public string GetTokenFromContextRequest(HttpContext context)
        {
            var tokenHeader = context.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();

            var token = bearerToken?.Split("Bearer ")[1];
            return token;
        }

        public Guid GetUserIdFromRequestContext(HttpContext context)
        {

            string bearerToken = GetTokenFromContextRequest(context);
            var tokenDecoded = _tokenHandler.GetTokenDataByStringValue(bearerToken);

            var userID = tokenDecoded.Claims.Where(W => W.Type == "userID").FirstOrDefault().Value;
            return Guid.Parse(userID.ToUpper());
        }

    }
}
