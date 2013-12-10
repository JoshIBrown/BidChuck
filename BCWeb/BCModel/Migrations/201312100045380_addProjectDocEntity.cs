namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProjectDocEntity : DbMigration
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
                        Url = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .Index(t => t.ProjectId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProjectDocument", new[] { "CompanyId" });
            DropIndex("dbo.ProjectDocument", new[] { "ProjectId" });
            DropForeignKey("dbo.ProjectDocument", "CompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ProjectDocument", "ProjectId", "dbo.Project");
            DropTable("dbo.ProjectDocument");
        }
    }
}
