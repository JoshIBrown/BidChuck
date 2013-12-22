namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Spatial;
    
    public partial class addLatLongToProjectAndCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyProfile", "GeoLocation", c => c.Geography());
            AddColumn("dbo.Project", "GeoLocation", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "GeoLocation");
            DropColumn("dbo.CompanyProfile", "GeoLocation");
        }
    }
}
