using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using HQF.Tutorial.WebAPI.Filters.Exceptions;
using HQF.Tutorial.WebAPI.GlobalStuffs.Exceptions.NLog;
using System.Web.Mvc;

namespace HQF.Tutorial.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Register Exception Loger
            RegisterExceptionLogers(config);

            //Enable CORS
            //config.EnableCors();
            var cors = new EnableCorsAttribute("http://localhost:31272", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();


            AreaRegistration.RegisterAllAreas();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           
        }

        private static void RegisterExceptionLogers(HttpConfiguration config)
        {
            //global exception handling
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
            //filter execption handling
            config.Filters.Add(new NotImplExceptionFilterAttribute());
        }


    }
}
