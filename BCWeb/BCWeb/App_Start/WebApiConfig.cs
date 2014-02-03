using System.Web.Http;

namespace BCWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            config.Routes.MapHttpRoute(
                         name: "CompaniesNearProject",
                         routeTemplate: "api/Companies/Search/ProjectIdForLocation/{projectIdForLocation}",
                         defaults: new { controller = "Companies", action = "GetForProject", projectIdForLocation = RouteParameter.Optional }
                         );

            config.Routes.MapHttpRoute(
                         name: "CompaniesDoingBidPackageScopes",
                         routeTemplate: "api/Companies/Search/BidPackageIdForScopes/{bidPackageIdForScopes}",
                         defaults: new { controller = "Companies", action = "GetForBidPackage", bidPackageIdForScopes = RouteParameter.Optional }
                         );

            config.Routes.MapHttpRoute(
                name: "ProjectProffer",
                routeTemplate: "api/Projects/{projectId}/Proffer/{bidPackageId}",
                defaults: new { controller = "ProjectProffer", bidPackageId = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "BidPackageInvitations",
                routeTemplate: "api/Projects/{projectId}/Invitations/{bidPackageId}",
                defaults: new { controller = "BidPackageInvitations", bidPackageId = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}