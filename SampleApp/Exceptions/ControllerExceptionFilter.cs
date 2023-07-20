using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApp.Exceptions
{
    public class ControllerExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode;
            switch (context.Exception)
            {
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Result = new ObjectResult(context.Exception.Message) { StatusCode = statusCode };
        }
    }
}
