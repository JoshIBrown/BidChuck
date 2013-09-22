using System.Web.Mvc;

namespace BCWeb.Areas.Projects
{
    public class ProjectsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Projects";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Project_default",
                "Project/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, new { controller = @"BidPackage" }
            );
        }
    }
}
