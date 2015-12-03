using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace HQF.Tutorial.WebAPI.PerRequestHandler
{
    public class TraceHelperHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            object perRequestTrace;
            if (request.Properties.TryGetValue("perRequestTrace", out perRequestTrace))
            {
                var trace = perRequestTrace as List<string>;
                if (trace != null)
                {
                    for (var i = 0; i < trace.Count; i++)
                    {
                        response.Headers.Add("Trace" + i.ToString("D2"), trace[i]);
                    }
                }

                //var trace = perRequestTrace as List<string>;
                if (trace != null)
                {
                    var traceResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<List<string>>(trace, new JsonMediaTypeFormatter())
                    };

                    var combinedResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new MultipartContent("mixed")
                        {
                            new HttpMessageContent(traceResponse),
                            new HttpMessageContent(response)
                        }
                    };

                    return combinedResponse;
                }
            }

            return response;
        }
    }
}