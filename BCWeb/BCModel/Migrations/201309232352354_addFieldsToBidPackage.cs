namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldsToBidPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackage", "DocLink", c => c.String());
            AddColumn("dbo.BidPackage", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BidPackage", "Notes");
            DropColumn("dbo.BidPackage", "DocLink");
        }
    }
}
