namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProjectsAndBids : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.BidPackage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Bid",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BidPackageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.BidPackageId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BidPackageXScope",
                c => new
                    {
                        BidPackageId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BidPackageId, t.ScopeId })
                .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.BidPackageId)
                .Index(t => t.ScopeId);
            
            AddColumn("dbo.Scope", "BidPackage_Id", c => c.Int());
            AddForeignKey("dbo.Scope", "BidPackage_Id", "dbo.BidPackage", "Id");
            CreateIndex("dbo.Scope", "BidPackage_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BidPackageXScope", new[] { "ScopeId" });
            DropIndex("dbo.BidPackageXScope", new[] { "BidPackageId" });
            DropIndex("dbo.Bid", new[] { "UserId" });
            DropIndex("dbo.Bid", new[] { "BidPackageId" });
            DropIndex("dbo.BidPackage", new[] { "ProjectId" });
            DropIndex("dbo.Project", new[] { "CreatorId" });
            DropIndex("dbo.Scope", new[] { "BidPackage_Id" });
            DropForeignKey("dbo.BidPackageXScope", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.BidPackageXScope", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.Bid", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Bid", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.BidPackage", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Project", "CreatorId", "dbo.UserProfile");
            DropForeignKey("dbo.Scope", "BidPackage_Id", "dbo.BidPackage");
            DropColumn("dbo.Scope", "BidPackage_Id");
            DropTable("dbo.BidPackageXScope");
            DropTable("dbo.Bid");
            DropTable("dbo.BidPackage");
            DropTable("dbo.Project");
        }
    }
}
