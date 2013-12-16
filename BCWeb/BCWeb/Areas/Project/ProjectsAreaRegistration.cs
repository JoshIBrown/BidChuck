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
                name: "Project_default",
                url: "Project/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = @"Bid|BidPackage|Document|Invitation" },
                namespaces: new[] { "BCWeb.Areas.Project.Controllers" }
            );
        }
    }
}
