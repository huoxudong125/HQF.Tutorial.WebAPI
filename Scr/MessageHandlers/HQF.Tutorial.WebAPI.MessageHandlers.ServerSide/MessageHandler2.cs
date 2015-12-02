using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HQF.Tutorial.WebAPI.MessageHandlers.ServerSide
{
    //A delegating handler can also skip the inner handler and directly create the response:
    public class MessageHandler2 : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Create the response.
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello!")
            };

            // Note: TaskCompletionSource creates a task that does not contain a delegate.
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);   // Also sets the task state to "RanToCompletion"
            return tsc.Task;
        }
    }
}
