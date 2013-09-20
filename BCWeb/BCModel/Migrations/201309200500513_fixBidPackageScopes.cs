namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixBidPackageScopes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scope", "BidPackage_Id", "dbo.BidPackage");
            DropIndex("dbo.Scope", new[] { "BidPackage_Id" });
            DropColumn("dbo.Scope", "BidPackage_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scope", "BidPackage_Id", c => c.Int());
            CreateIndex("dbo.Scope", "BidPackage_Id");
            AddForeignKey("dbo.Scope", "BidPackage_Id", "dbo.BidPackage", "Id");
        }
    }
}
