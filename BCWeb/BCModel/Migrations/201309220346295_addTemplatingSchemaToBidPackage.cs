namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTemplatingSchemaToBidPackage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackage", "TemplateBidPackageId", c => c.Int());
            AddForeignKey("dbo.BidPackage", "TemplateBidPackageId", "dbo.BidPackage", "Id");
            CreateIndex("dbo.BidPackage", "TemplateBidPackageId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BidPackage", new[] { "TemplateBidPackageId" });
            DropForeignKey("dbo.BidPackage", "TemplateBidPackageId", "dbo.BidPackage");
            DropColumn("dbo.BidPackage", "TemplateBidPackageId");
        }
    }
}
