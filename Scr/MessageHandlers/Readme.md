##Adding a Handler to the Pipeline(Server Side)

To add a message handler on the server side, add the handler to the HttpConfiguration.MessageHandlers collection. 
you can do this inside the WebApiConfig class:

``` c#
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.MessageHandlers.Add(new MessageHandler1());
        config.MessageHandlers.Add(new MessageHandler2());

        // Other code not shown...
    }
}
```

Message handlers are called in the same order that they appear in **MessageHandlers** collection. Because they are nested, the response message travels in the other direction. That is, the last handler is the first to get the response message.

>Notice that you don't need to set the inner handlers; the Web API framework automatically connects the message handlers.

If you are self-hosting, create an instance of the HttpSelfHostConfiguration class and add the handlers to the **MessageHandlers** collection.

``` c#
var config = new HttpSelfHostConfiguration("http://localhost");
config.MessageHandlers.Add(new MessageHandler1());
config.MessageHandlers.Add(new MessageHandler2());
```


##Adding Message Handlers to the Client Pipeline(Client Side)

To add custom handlers to `HttpClient`, use the `HttpClientFactory.Create` method:

```c#
HttpClient client = HttpClientFactory.Create(new Handler1(), new Handler2(), new Handler3());
```

Message handlers are called in the order that you pass them into the `Create` method. Because handlers are nested, the response message travels in the other direction. That is, the last handler is the first to get the response message.

##More Details
[HTTP Message Handlers in ASP.NET Web API](http://www.asp.net/web-api/overview/advanced/http-message-handlers)    
[HttpClient Message Handlers in ASP.NET Web API](http://www.asp.net/web-api/overview/advanced/httpclient-message-handlers) 