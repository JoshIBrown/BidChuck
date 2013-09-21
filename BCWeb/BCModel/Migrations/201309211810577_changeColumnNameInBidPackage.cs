namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class changeColumnNameInBidPackage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BidPackage", "CreatorId", "dbo.CompanyProfile");
            DropIndex("dbo.BidPackage", new[] { "CreatorId" });
            RenameColumn("dbo.BidPackage", "CreatorId", "CreatedById");
            AddForeignKey("dbo.BidPackage", "CreatedById", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.BidPackage", "CreatedById");

        }

        public override void Down()
        {
            DropIndex("dbo.BidPackage", new[] { "CreatedById" });
            DropForeignKey("dbo.BidPackage", "CreatedById", "dbo.CompanyProfile");
            RenameColumn("dbo.BidPackage", "CreatedById", "CreatorId");
            CreateIndex("dbo.BidPackage", "CreatorId");
            AddForeignKey("dbo.BidPackage", "CreatorId", "dbo.CompanyProfile", "Id");
        }
    }
}
