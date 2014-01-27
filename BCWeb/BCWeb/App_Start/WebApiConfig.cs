using System.Web.Http;

namespace BCWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            config.Routes.MapHttpRoute(
                name: "BidPackageInvitationsPut",
                routeTemplate: "api/Projects/{id}/Invitations/{bidPackageId}",
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