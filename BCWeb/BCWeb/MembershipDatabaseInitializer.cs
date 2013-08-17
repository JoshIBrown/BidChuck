using BCWeb.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data;
using System;
using System.Data.OleDb;
using BCModel;
using System.Collections.Generic;

namespace BCWeb
{
    public class MembershipDatabaseInitializer : CreateDatabaseIfNotExists<BidChuckContext>
    {
        protected override void Seed(BidChuckContext context)
        {

            LoadScopes(context);
        }

        public void LoadScopes(BidChuckContext context)
        {
            //using (BidChuckContext context = new BidChuckContext())
            //{
            ////load scopes
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes';", path);
            string commandText = "select distinct [GroupNumber], [GroupDescription],[2ndTierSortNumber], [2ndTierSortDescription],[3rdTierSortNumber],[3rdTierSortDescription] from tblCSI2004.csv as [a]";
            List<Scope> scopes = new List<Scope>();
            Scope parent;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter adap = new OleDbDataAdapter(commandText, conn))
                {
                    conn.Open();
                    using (DataSet ds = new DataSet())
                    {
                        adap.Fill(ds);
                        foreach (DataTable table in ds.Tables)
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                //Scope s = new Scope();
                                // is root when 1st,2nd and third descriptorare equal 
                                // and when not already in temp scope list
                                if (row["GroupNumber"].ToString().Trim() == row["2ndTierSortNumber"].ToString().Trim()
                                    && row["GroupNumber"].ToString().Trim() == row["3rdTierSortNumber"].ToString().Trim()
                                    && scopes.Where(x => x.Number == row["GroupNumber"].ToString().Trim()).Count() == 0)
                                {
                                    scopes.Add(new Scope
                                    {
                                        Description = row["GroupDescription"].ToString().Trim(),
                                        Number = row["GroupNumber"].ToString().Trim()
                                    });
                                }
                                // is child when 1st and 2nd are not equal
                                // and when not already in temp scope list
                                if (row["GroupNumber"].ToString().Trim() != row["2ndTierSortNumber"].ToString().Trim()
                                    && scopes.Where(x => x.Number == row["2ndTierSortNumber"].ToString().Trim()).Count() == 0)
                                {
                                    parent = scopes.Where(x => x.Number == row["GroupNumber"].ToString().Trim()).FirstOrDefault();
                                    scopes.Add(new Scope
                                    {
                                        Description = row["2ndTierSortDescription"].ToString().Trim(),
                                        Number = row["2ndTierSortNumber"].ToString().Trim(),
                                        Parent = parent
                                    });
                                }
                                // is grandchild when 2nd and 3rd are not equal
                                // and when not already in temp scope list
                                if (/*row["GroupNumber"].ToString().Trim() == row["2ndTierSortNumber"].ToString().Trim()
                                    &&*/ row["2ndTierSortNumber"].ToString().Trim() != row["3rdTierSortNumber"].ToString().Trim()
                                    && scopes.Where(x => row["3rdTierSortNumber"].ToString().Trim().Contains(x.Number)).Count() == 0)
                                {
                                    parent = scopes.Where(x => x.Number == row["2ndTierSortNumber"].ToString().Trim()).FirstOrDefault();
                                    scopes.Add(new Scope
                                    {
                                        Description = row["3rdTierSortDescription"].ToString().Trim(),
                                        Number = row["3rdTierSortNumber"].ToString().Trim(),
                                        Parent = parent
                                    });
                                }
                                //context.Scopes.Add(s);
                            }
                        }
                        //context.SaveChanges();

                        foreach (Scope s in scopes)
                        {
                            context.Scopes.Add(s);
                        }
                        context.SaveChanges();
                    }
                }
            }
            //}
        }

    }
}