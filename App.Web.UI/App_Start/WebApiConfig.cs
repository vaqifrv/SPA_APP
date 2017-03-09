using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Web.UI.Infrastructure;

namespace App.Web.UI
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
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "DefaultApi2",
              routeTemplate: "api/{controller}/{action}/{itemId}",
              defaults: new { itemId = RouteParameter.Optional }
              );

            config.Formatters.Clear();

            config.Formatters.Insert(0, new JsonNetFormatter());
            config.Formatters.Add(new PdfFormatter());
        }
    }
}
