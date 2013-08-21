using BCModel;
using BCWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace BCWeb
{

    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //System.IO.Directory.CreateDirectory(@"C:\email\");
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialise();

            Database.SetInitializer<BidChuckContext>(new MembershipDatabaseInitializer());
            BidChuckContext context = new BidChuckContext();
            context.Database.Initialize(true);


            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserID", "Email", autoCreateTables: true);
            }


            // Application specific
            if (!Roles.Provider.RoleExists("Administrator"))
            {
                Roles.Provider.CreateRole("Administrator");
            }

            if (Membership.Provider.GetUser("admin", false) == null)
            {
                ((SimpleMembershipProvider)Membership.Provider).CreateUserAndAccount("admin",
                    "bidchuck",
                    false,
                    new Dictionary<string, object> { 
                    { "Published", false },
                    {"Phone",""},
                    {"CompanyName","bidchuck"},
                    {"State","bidchuck"},
                    {"County","bidchuck"} 
                    });
            }

            if (!Roles.Provider.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.Provider.AddUsersToRoles(new[] { "admin" }, new[] { "Administrator" });
            }


        }
    }
}