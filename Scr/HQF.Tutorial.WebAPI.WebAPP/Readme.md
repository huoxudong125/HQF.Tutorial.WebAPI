
#Enabling Cross-Origin Requests in ASP.NET Web API 2

同源策略（Same Origin Policy）的存在导致了“源”自A的脚本只能操作“同源”页面的DOM，“跨源”操作来源于B的页面将会被拒绝。同源策略以及跨域资源共享在大部分情况下针对的是Ajax请求。同源策略主要限制了通过XMLHttpRequest实现的Ajax请求，如果请求的是一个“异源”地址，浏览器将不允许读取返回的内容。JSONP是一种常用的解决跨域资源共享的解决方案，我们还可以利用ASP.NET Web API自身的扩展性提供一种“通用”的JSONP实现方案。此外就是微软还提供一个更加完整的方案


>`Cross Origin Resource Sharing` (CORS) is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others. CORS is safer and more flexible than earlier techniques such as	JSONP. This tutorial shows how to enable CORS in your Web API application.

[Here](http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api) is Article from www.asp.net

##Solution 1
JSONP仅仅是利用`<script>`的src标签加载的脚本不受同源策略约束而采取的一种编程技巧，其本身并不是一种官方协议。并且并非所有类型跨域调用都能采用JSONP的方式来解决（由于所有具有src属性的HTML标签均通过HTTP-GET的方式来加载目标资源，这决定了JSONP只适用于HTTP-GET请求），所以我们必须寻求一种更好的解决方案。

``` html
  <html>
  <head>
      <title>联系人列表</title>
      <script type="text/javascript" src="@Url.Content("~/scripts/jquery-1.10.2.js")"></script>
      <script type="text/javascript">
          function listContacts(contacts)
          {
              $.each(contacts, function (index, contact) {
                  var html = "<li><ul>";
                  html += "<li>Name: " + contact.Name + "</li>";
                  html += "<li>Phone No:" + contact.PhoneNo + "</li>";
                  html += "<li>Email Address: " + contact.EmailAddress + "</li>";
                  html += "</ul>";
                  $("#contacts").append($(html));
              });
          }
      </script>
  </head>
  <body>
      <ul id="contacts"></ul>
      <script type="text/javascript" src="http://localhost:3721/api/contacts?callback=listContacts"></script>
  </body>
  </html>

```

##Solution 2

我们通过继承JsonMediaTypeFormatter定义了如下一个JsonpMediaTypeFormatter类型。它的只读属性Callback代表JavaScript回调函数名称，改属性在构造函数中指定。在重写的方法WriteToStreamAsync中，对于非JSONP调用（回调函数不存在），我们直接调用基类的同名方法对响应对象实施针对JSON的序列化，否则调用WriteToStream方法将对象序列化后的JSON字符串填充到JavaScript回调函数中。

```c#

public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
  {
      public string Callback { get; private set; }
   
      public JsonpMediaTypeFormatter(string callback = null)
      {
          this.Callback = callback;
      }
   
      public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
      {
          if (string.IsNullOrEmpty(this.Callback))
          {
              return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
          }
          try
          {
              this.WriteToStream(type, value, writeStream, content);
              return Task.FromResult<AsyncVoid>(new AsyncVoid());
          }
          catch (Exception exception)
          {
              TaskCompletionSource<AsyncVoid> source = new TaskCompletionSource<AsyncVoid>();
              source.SetException(exception);
              return source.Task;
          }
      }
   
      private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
      {
          JsonSerializer serializer = JsonSerializer.Create(this.SerializerSettings);
          using(StreamWriter streamWriter = new StreamWriter(writeStream, this.SupportedEncodings.First()))
          using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter) { CloseOutput = false })
          {
              jsonTextWriter.WriteRaw(this.Callback + "(");
              serializer.Serialize(jsonTextWriter, value);
              jsonTextWriter.WriteRaw(")");
          }
      }
   
      public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
      {
          if (request.Method != HttpMethod.Get)
          {
              return this;
          }
          string callback;
          if (request.GetQueryNameValuePairs().ToDictionary(pair => pair.Key, 
               pair => pair.Value).TryGetValue("callback", out callback))
          {
              return new JsonpMediaTypeFormatter(callback);
          }
          return this;
      }
   
      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct AsyncVoid
      {}
  }
```

##Solution 3



```
Install-Package Microsoft.AspNet.WebApi.Cors 
```

```
using System.Web.Http;
namespace WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // New code
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
```
```
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebService.Controllers
{
    [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        // Controller methods not shown...
    }
}
```

>For the origins parameter, use the URI where you deployed the WebClient application. This allows cross-origin requests from WebClient, while still disallowing all other cross-domain requests. Later, I’ll describe the parameters for [EnableCors] in more detail.
>Do not include a forward slash at the end of the origins URL.
>Redeploy the updated WebService application. You don't need to update WebClient. Now the AJAX request from WebClient should succeed. The GET, PUT, and POST methods are all allowed.





##References
[[CORS：跨域资源共享] 同源策略与JSONP](http://www.cnblogs.com/artech/p/cors-4-asp-net-web-api-01.html)    
[ASP.NET Web API 跨域访问（CORS）要注意的地方](http://edi.wang/post/2013/12/27/tips-for-aspnet-webapi-cors) 

