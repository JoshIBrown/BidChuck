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
