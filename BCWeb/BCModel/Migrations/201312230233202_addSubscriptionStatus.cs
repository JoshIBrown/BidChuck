namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addSubscriptionStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyProfile", "SubscriptionStatus", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.CompanyProfile", "SubscriptionStatus");
        }
    }
}
