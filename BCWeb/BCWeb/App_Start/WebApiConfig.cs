using System.Web.Http;

namespace BCWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "SelectedScopesApi",
            //    routeTemplate: "api/Scopes/GetSelectedScopes/{type}/{id}",
            //    defaults: new { controller = "Scopes", action = "GetSelectedScopes", type = RouteParameter.Optional, id = RouteParameter.Optional },
            //    constraints: new { controller = @"Scopes" }

            //);
            config.Routes.MapHttpRoute(
                name: "Api_wAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}