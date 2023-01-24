using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using openglclevel_server_models.Exceptions;

namespace openglclevel_server_api.Filters
{
    public class AutomaticExceptionHandler : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            switch (context.Exception)
            {
                case FriendlyException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case UserSessionException:
                    statusCode = HttpStatusCode.Conflict;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            //if (context.Exception is UserFoundException)
            //{
            //    statusCode = HttpStatusCode.Conflict;
            //}

            //if (context.Exception is DatabaseException)
            //{
            //    statusCode = HttpStatusCode.UnprocessableEntity;
            //}
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(new
            {
                ErrorMessages = new List<string> { context.Exception.Message },
                AdditionalInformation = context.Exception.InnerException != null ? context.Exception.InnerException.Message : "",
            });
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }
    }
}
