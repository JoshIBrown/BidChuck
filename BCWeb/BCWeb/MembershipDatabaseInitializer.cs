using BCWeb.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;

namespace BCWeb
{
    public class MembershipDatabaseInitializer : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserID", "Email", autoCreateTables: true);

            if (!Roles.Provider.RoleExists("Administrator"))
            {
                Roles.Provider.CreateRole("Administrator");
            }

            if (Membership.Provider.GetUser("admin", false) == null)
            {
                ((SimpleMembershipProvider)Membership.Provider).CreateUserAndAccount("admin", "bidchuck");
            }

            if (!Roles.Provider.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.Provider.AddUsersToRoles(new[] { "admin" }, new[] { "Administrator" });
            }
        }
    }
}