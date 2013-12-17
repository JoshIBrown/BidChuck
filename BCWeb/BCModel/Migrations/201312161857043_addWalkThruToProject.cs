namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWalkThruToProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .Index(t => t.ProjectId)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Project", "WalkThruDateTime", c => c.DateTime());
            AddColumn("dbo.Project", "NoWalkThru", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "WalkThruTBD", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidPackage", "UseProjectBidDateTime", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidPackage", "UseProjectWalkThruDateTime", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidPackage", "WalkThruDateTime", c => c.DateTime());
            AddColumn("dbo.BidPackage", "NoWalkThru", c => c.Boolean(nullable: false));
            AddColumn("dbo.BidPackage", "WalkThruTBD", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BidPackage", "BidDateTime", c => c.DateTime());
            DropColumn("dbo.BidPackage", "DocLink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BidPackage", "DocLink", c => c.String());
            DropIndex("dbo.ProjectDocument", new[] { "CompanyId" });
            DropIndex("dbo.ProjectDocument", new[] { "ProjectId" });
            DropForeignKey("dbo.ProjectDocument", "CompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ProjectDocument", "ProjectId", "dbo.Project");
            AlterColumn("dbo.BidPackage", "BidDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.BidPackage", "WalkThruTBD");
            DropColumn("dbo.BidPackage", "NoWalkThru");
            DropColumn("dbo.BidPackage", "WalkThruDateTime");
            DropColumn("dbo.BidPackage", "UseProjectWalkThruDateTime");
            DropColumn("dbo.BidPackage", "UseProjectBidDateTime");
            DropColumn("dbo.Project", "WalkThruTBD");
            DropColumn("dbo.Project", "NoWalkThru");
            DropColumn("dbo.Project", "WalkThruDateTime");
            DropTable("dbo.ProjectDocument");
        }
    }
}
