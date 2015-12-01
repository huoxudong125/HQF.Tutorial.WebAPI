using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace HQF.Tutorial.WebAPI.Filters.Exceptions
{
    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                {
                    Content = new StringContent("Has filted by NotImplExceptionFilterAttribute by HQF")
                };
                
            }
        }
    }
}
