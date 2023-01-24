using Microsoft.AspNetCore.Mvc.Filters;
using openglclevel_server_api.UtilControllers;
using openglclevel_server_models;
using openglclevel_server_models.Exceptions;

namespace openglclevel_server_api
{
    public class UserActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var userToken = ControllerUtilities
                    .GetUserIdFromRequestContext(filterContext.HttpContext);

            var userID = filterContext.HttpContext.Session.GetString("userID");

            if (userID != null)
            {
                if (userToken == Guid.Parse(userID))
                    StaticMemoryVariables.UserID = Guid.Parse(userID);
                else
                    throw new UserSessionException("Provided token do not correspond to session");
            }
            else
            {
                StaticMemoryVariables.UserID = userToken;
            }
        }
    }
}
