using IncidentManagement.API.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IncidentManagement.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate:"api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            //Configuring custom CustomActionFilterAttribute
            //config.Filters.Add(new CustomActionFilterAttribute());

            //Configuring custom HandleAPIExceptionAttribute
            //config.Filters.Add(new HandleAPIExceptionAttribute());
        }
    }
}
