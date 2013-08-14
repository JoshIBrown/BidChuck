using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.OleDb;
using System.Data;

namespace BCWeb.Tests
{
    [TestClass]
    public class ScopeFileTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            ////load scopes
            string path = @"../../";
            string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes';", path);
            string commandText = "select distinct [GroupNumber], [GroupDescription],[2ndTierSortNumber], [2ndTierSortDescription],[3rdTierSortNumber],[3rdTierSortDescription] from tblCSI2004.csv as [a]";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter adap = new OleDbDataAdapter(commandText, conn))
                {
                    conn.Open();
                    using (DataSet ds = new DataSet())
                    {
                        adap.Fill(ds);
                        Assert.AreEqual(1, ds.Tables.Count);
                        Assert.AreNotEqual(5, ds.Tables[0].Rows.Count);
                        //foreach (DataTable table in ds.Tables)
                        //{
                        //    foreach (DataRow row in table.Rows)
                        //    {

                        //    }
                        //}
                    }
                }
            }
        }
    }
}
