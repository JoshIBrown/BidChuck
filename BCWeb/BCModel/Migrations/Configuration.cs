namespace BCModel.Migrations
{
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
            //string path = AppDomain.CurrentDomain.BaseDirectory.Replace("BCWeb\\bin\\",string.Empty) + "BCModel\\Migrations\\";
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

        }
    }
}
