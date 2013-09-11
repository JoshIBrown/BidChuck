namespace BCModel.Migrations
{
    using BCModel.Projects;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.OleDb;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BCModel.BidChuckContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BCModel.BidChuckContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //DbSet<State> states = context.State;
            //DbSet<County> counties = context.Counties;
            //string path = AppDomain.CurrentDomain.BaseDirectory.Replace("BCWeb\\bin\\", string.Empty) + "BCModel\\Migrations\\";
            //string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes';", path);
            //string commandText = "select [ID],[Code],[Name] from state.csv";
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
            //                    State toAdd = new State { Id = int.Parse(row["ID"].ToString()), Abbr = row["Code"].ToString(), Name = row["Name"].ToString() };
            //                    states.AddOrUpdate(toAdd);
            //                }
            //            }
            //        }
            //    }
            //}
            //context.SaveChanges();
            //commandText = "select [ID],[StateID],[Name] from county.csv";

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
            //                    County toAdd = new County { Id = int.Parse(row["ID"].ToString()), StateId = int.Parse(row["StateID"].ToString()), Name = row["Name"].ToString() };
            //                    counties.AddOrUpdate(toAdd);
            //                }
            //            }
            //        }
            //    }
            //}
            //context.SaveChanges();
            if (context.BusinessTypes.Count() == 0)
            {
                context.BusinessTypes.Add(new BusinessType { Name = "General Contractor" });
                context.BusinessTypes.Add(new BusinessType { Name = "Sub-Contractor" });
                context.BusinessTypes.Add(new BusinessType { Name = "Architect" });
                context.BusinessTypes.Add(new BusinessType { Name = "Engineer" });
                context.BusinessTypes.Add(new BusinessType { Name = "Owner/Client" });
                context.BusinessTypes.Add(new BusinessType { Name = "Materials Vendor" });
                context.BusinessTypes.Add(new BusinessType { Name = "Materials Manufacturer" });
                context.BusinessTypes.Add(new BusinessType { Name = "Consultant" });
                context.SaveChanges();
            }

            if (context.ProjectTypes.Count() == 0)
            {
                context.ProjectTypes.Add(new Projects.ProjectType { Name = "Federal Government" });
                context.ProjectTypes.Add(new Projects.ProjectType { Name = "State Government" });
                context.ProjectTypes.Add(new Projects.ProjectType { Name = "Local Government" });
                context.ProjectTypes.Add(new Projects.ProjectType { Name = "Private" });
                context.ProjectTypes.Add(new Projects.ProjectType { Name = "Private - Non-Profit" });
                context.SaveChanges();
            }

            if (context.ConstructionTypes.Count() == 0)
            {
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE I-A--Fire Resistive Non-combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE I-B--Fire Resistive Non-Combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE II-A--Protected Non-Combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE II-B--Unprotected Non-Combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE III-A--Protected Combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE III-B--Unprotected Combustible" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE IV--Heavy Timber" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE V-A--Protected Wood Frame" });
                context.ConstructionTypes.Add(new ConstructionType { Name = "TYPE V-B--Unprotected Wood Frame" });
                context.SaveChanges();
            }

            if (context.BuildingTypes.Count() == 0)
            {
                string path = "";
#if DEBUG
                path = AppDomain.CurrentDomain.BaseDirectory.Replace("BCWeb\\bin\\", string.Empty) + "BCModel\\Migrations\\";
#endif
#if !DEBUG
                path = AppDomain.CurrentDomain.BaseDirectory;
#endif
                string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes';", path);

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string commandText = "select distinct [1stTier] from BuildingTypes.csv";
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
                                    context.BuildingTypes.Add(new BuildingType { Name = row["1stTier"].ToString() });
                                }
                            }
                        }
                        
                    }
                    context.SaveChanges();
                    commandText = "select [1stTier],[2ndTier] from BuildingTypes.csv";
                    BuildingType parent;
                    using (OleDbDataAdapter adap = new OleDbDataAdapter(commandText, conn))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adap.Fill(ds);
                            foreach (DataTable table in ds.Tables)
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    var foo =row["1stTier"].ToString();
                                    parent = context.BuildingTypes.Where(x => x.Name == foo ).FirstOrDefault();
                                    context.BuildingTypes.Add(new BuildingType { Name = row["2ndTier"].ToString(), Parent = parent });
                                }
                            }
                        }
                    }
                    conn.Close();

                }
                context.SaveChanges();
            }
        }
    }
}
