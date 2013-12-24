using System.Web.Mvc;

namespace BCWeb.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "ManageUserScopes",
                "Account/User/{id}/Scopes/Manage",
                new { controller = "Scopes", action = "ManageUser", id = UrlParameter.Optional },
                new { controller = @"Scopes" },
                new[] { "BCWeb.Areas.Account.Controllers" }
            );

            context.MapRoute(
                "ManageCompanyScopes",
                "Account/Company/Scopes/Manage",
                new { controller = "Scopes", action = "ManageCompany" },
                new { controller = @"Scopes" },
                new[] { "BCWeb.Areas.Account.Controllers" }
            );

            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new { controller = @"Scopes|Users|Company" },
                new[] { "BCWeb.Areas.Account.Controllers" }
            );
        }
    }
}
