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
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialise();

            Database.SetInitializer<BidChuckContext>(new MembershipDatabaseInitializer());
            BidChuckContext context = new BidChuckContext();
            context.Database.Initialize(true);
            InitializeMembership();
        }

        private void InitializeMembership()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserID", "Email", autoCreateTables: true);
            }


            // Application specific
            if (!Roles.Provider.RoleExists("Administrator"))
            {
                Roles.Provider.CreateRole("Administrator");
            }

            if (!Roles.Provider.RoleExists("Manager"))
            {
                Roles.Provider.CreateRole("Manager");
            }

            if (!Roles.Provider.RoleExists("Employee"))
            {
                Roles.Provider.CreateRole("Employee");
            }

            if (!Roles.Provider.RoleExists("general_contractor"))
            {
                Roles.Provider.CreateRole("general_contractor");
            }

            if (!Roles.Provider.RoleExists("subcontractor"))
            {
                Roles.Provider.CreateRole("subcontractor");
            }

            if (!Roles.Provider.RoleExists("architect"))
            {
                Roles.Provider.CreateRole("architect");
            }

            if (!Roles.Provider.RoleExists("engineer"))
            {
                Roles.Provider.CreateRole("engineer");
            }

            if (!Roles.Provider.RoleExists("owner_client"))
            {
                Roles.Provider.CreateRole("owner_client");
            }

            if (!Roles.Provider.RoleExists("materials_vendor"))
            {
                Roles.Provider.CreateRole("materials_vendor");
            }

            if (!Roles.Provider.RoleExists("materials_manufacturer"))
            {
                Roles.Provider.CreateRole("materials_manufacturer");
            }

            if (!Roles.Provider.RoleExists("consultant"))
            {
                Roles.Provider.CreateRole("consultant");
            }


            if (Membership.Provider.GetUser("admin", false) == null)
            {
                ((SimpleMembershipProvider)Membership.Provider).CreateUserAndAccount("admin",
                    "bidchuck",
                    false,
                    new Dictionary<string, object> { 
                    { "Published", false },
                    {"Phone",""},
                    {"CompanyName","bidchuck"}
                    });
            }

            if (!Roles.Provider.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.Provider.AddUsersToRoles(new[] { "admin" }, new[] { "Administrator" });
            }
        }
    }
}