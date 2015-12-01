using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace HQF.Tutorial.WebAPI.Filters.Exceptions
{
    public class DivideByZeroExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is DivideByZeroException)
            {
                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content =
                        new StringContent(
                            "We apologize but an error occured within the application. Please try again later.",
                            Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}