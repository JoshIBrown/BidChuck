namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addDateAndFlagToBidPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackage", "BidDateTime", c => c.DateTime());
            AddColumn("dbo.BidPackage", "IsMaster", c => c.Boolean(nullable: false, defaultValue: true));
        }

        public override void Down()
        {
            DropColumn("dbo.BidPackage", "IsMaster");
            DropColumn("dbo.BidPackage", "BidDateTime");
        }
    }
}
