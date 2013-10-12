namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class renameFileInBid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "CompanyId", "dbo.CompanyProfile");
            DropIndex("dbo.Bid", new[] { "CompanyId" });
            RenameColumn("dbo.Bid", "CompanyId", "BiddingCompanyId");
            AddForeignKey("dbo.Bid", "BiddingCompanyId", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.Bid", "BiddingCompanyId");
        }

        public override void Down()
        {
            DropIndex("dbo.Bid", new[] { "BiddingCompanyId" });
            DropForeignKey("dbo.Bid", "BiddingCompanyId", "dbo.CompanyProfile");
            RenameColumn("dbo.Bid", "BiddingCompanyId", "CompanyId");
            CreateIndex("dbo.Bid", "CompanyId");
            AddForeignKey("dbo.Bid", "CompanyId", "dbo.CompanyProfile", "Id");
        }
    }
}
