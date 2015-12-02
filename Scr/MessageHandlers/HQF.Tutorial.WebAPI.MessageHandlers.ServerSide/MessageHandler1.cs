using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HQF.Tutorial.WebAPI.MessageHandlers.ServerSide
{
    public class MessageHandler1 : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Process request");
            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
            Debug.WriteLine("Process response");
            return response;
        }
    }
}
