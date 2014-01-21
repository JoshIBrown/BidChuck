using System.Web.Mvc;

namespace BCWeb.Areas.Contacts
{
    public class contactsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contacts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Contacts_default",
                "Contacts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new { controller = @"Request" },
                new[] { "BCWeb.Areas.Contacts" }
            );
        }
    }
}
