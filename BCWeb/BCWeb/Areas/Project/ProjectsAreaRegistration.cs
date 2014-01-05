using System.Web.Mvc;

namespace BCWeb.Areas.Project
{
    public class ProjectAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Project";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReceivedBids",
                "Project/{id}/Bid/Received",
                new { controller = "Bid", action = "Received", id = UrlParameter.Optional },
                new { controller = @"Bid", action = "Received" },
                new[] { @"BCWeb.Areas.Project.Controllers" }
                );

            context.MapRoute(
                "InvitationRequests",
                "Project/{id}/Invitation/Requests",
                new { controller = "Invitation", action = "Requests", id = UrlParameter.Optional },
                new { controller = @"Invitation", action = "Requests" },
                new[] { @"BCWeb.Areas.Project.Controllers" }
                );

            context.MapRoute(
                "BidPackageInvite",
                "Project/BidPackage/{bidPackageId}/Invitation/Send",
                new { controller = "Invitation", action = "SendForBidPackage", bidPackageId = UrlParameter.Optional },
                new { controller = @"Invitation", action = "SendForBidPackage" },
                new[] { @"BCWeb.Areas.Project.Controllers" }
            );


            context.MapRoute(
                "Project_default",
                "Project/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new { controller = @"BidPackage|Invitation|Bid|Document" },
                new[] { @"BCWeb.Areas.Project.Controllers" }
            );
        }
    }
}
