#Global Exception Handling
>Some unhandled exceptions can be processed via exception filters, but there are a number of cases that exception filters can’t handle. For example:
>1. Exceptions thrown from controller constructors.
>2. Exceptions thrown from message handlers.
>3. Exceptions thrown during routing.
>4. Exceptions thrown during response content serialization.

##Solution Overview
We provide two new user-replaceable services, `IExceptionLogger` and `IExceptionHandler`, to log and handle unhandled exceptions. The services are very similar, with two main differences:

1. We support registering multiple exception loggers but only a single exception handler.
2. Exception loggers always get called, even if we’re about to abort the connection. Exception handlers only get called when we’re still able to choose which response message to send.

Both services provide access to an exception context containing relevant information from the point where the exception was detected, particularly the HttpRequestMessage, the HttpRequestContext, the thrown exception and the exception source (details below).

##When to Use

Exception loggers are the solution to seeing all unhandled exception caught by Web API.
Exception handlers are the solution for customizing all possible responses to unhandled exceptions caught by Web API.
Exception filters are the easiest solution for processing the subset unhandled exceptions related to a specific action or controller.


##References
[elmah-web-api-log-all-exceptions](https://github.com/cornflourblue/elmah-web-api-log-all-exceptions)  
[A Global Error Handling, Logging, and Notification Class Library for Data Access Web API](http://www.codeproject.com/Articles/1060422/A-Global-Error-Handling-Logging-and-Notification-C?display=Print)  