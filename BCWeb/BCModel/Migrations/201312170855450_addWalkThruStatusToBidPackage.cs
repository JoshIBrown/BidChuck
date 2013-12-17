namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWalkThruStatusToBidPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackage", "WalkThruStatus", c => c.Int(nullable: false));
            DropColumn("dbo.BidPackage", "NoWalkThru");
            DropColumn("dbo.BidPackage", "WalkThruTBD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BidPackage", "WalkThruTBD", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidPackage", "NoWalkThru", c => c.Boolean(nullable: false));
            DropColumn("dbo.BidPackage", "WalkThruStatus");
        }
    }
}
