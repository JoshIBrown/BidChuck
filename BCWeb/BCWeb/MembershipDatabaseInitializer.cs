using BCWeb.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using System;
using System.Data.OleDb;

namespace BCWeb
{
    public class MembershipDatabaseInitializer : CreateDatabaseIfNotExists<UsersContext>
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
                ((SimpleMembershipProvider)Membership.Provider).CreateUserAndAccount("admin", "bidchuck", false);
            }

            if (!Roles.Provider.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.Provider.AddUsersToRoles(new[] { "admin" }, new[] { "Administrator" });
            }

            ////load scopes
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes';", path);
            //string commandText = "select distinct [GroupNumber], [GroupDescription], [2ndTierSortNumber], [2ndTierSortDescription], [3rdTierSortNumber], [3rdTierSortDescription] from tblCSI2004.csv";
            //using (OleDbConnection conn = new OleDbConnection(connectionString))
            //{
            //    using (OleDbDataAdapter adap = new OleDbDataAdapter(commandText, conn))
            //    {
            //        conn.Open();
            //        using (DataSet ds = new DataSet())
            //        {
            //            adap.Fill(ds);
            //            foreach (DataTable table in ds.Tables)
            //            {
            //                foreach (DataRow row in table.Rows)
            //                {
            //                    Scope s = new Scope();
            //                    s.FirstTierSortNumber = (string)row["GroupNumber"];
            //                    s.FirstTierSortDescription = (string)row["GroupDescription"];                                
            //                    s.SecondTierSortNumber = (string)row["2ndTierSortNumber"];
            //                    s.SecondTierSortDescription = (string)row["2ndTierSortDescription"];
            //                    s.ThirdTierSortNumber = (string)row["3rdTierSortNumber"];
            //                    s.ThirdTierSortDescription = (string)row["3rdTierSortDescription"];
            //                    context.Scopes.Add(s);
            //                }
            //            }
            //            context.SaveChanges();
            //        }
            //    }
            //}
        }
    }
}