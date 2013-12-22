namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class makeWalkThruStatusNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BidPackage", "WalkThruStatus", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            AlterColumn("dbo.BidPackage", "WalkThruStatus", c => c.Int(nullable: false));
        }
    }
}
