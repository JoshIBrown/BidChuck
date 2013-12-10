namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeDocLinkFromBidPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackage", "UseProjectBidDateTime", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.BidPackage", "UseProjectWalkThruDateTime", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.BidPackage", "WalkThruDateTime", c => c.DateTime());
            AddColumn("dbo.BidPackage", "NoWalkThru", c => c.Boolean(nullable: false, defaultValue:false));
            AddColumn("dbo.BidPackage", "WalkThruTBD", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.BidPackage", "BidDateTime", c => c.DateTime());
            DropColumn("dbo.BidPackage", "DocLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BidPackage", "DocLink", c => c.String());
            AlterColumn("dbo.BidPackage", "BidDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.BidPackage", "WalkThruTBD");
            DropColumn("dbo.BidPackage", "NoWalkThru");
            DropColumn("dbo.BidPackage", "WalkThruDateTime");
            DropColumn("dbo.BidPackage", "UseProjectWalkThruDateTime");
            DropColumn("dbo.BidPackage", "UseProjectBidDateTime");
        }
    }
}
