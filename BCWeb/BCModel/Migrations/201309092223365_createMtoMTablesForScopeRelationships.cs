namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createMtoMTablesForScopeRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScopeUserProfile", "Scope_Id", "dbo.Scope");
            DropForeignKey("dbo.ScopeUserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ScopeCompanyProfile", "Scope_Id", "dbo.Scope");
            DropForeignKey("dbo.ScopeCompanyProfile", "CompanyProfile_Id", "dbo.CompanyProfile");
            DropIndex("dbo.ScopeUserProfile", new[] { "Scope_Id" });
            DropIndex("dbo.ScopeUserProfile", new[] { "UserProfile_UserId" });
            DropIndex("dbo.ScopeCompanyProfile", new[] { "Scope_Id" });
            DropIndex("dbo.ScopeCompanyProfile", new[] { "CompanyProfile_Id" });
            CreateTable(
                "dbo.CompanyXScope",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ScopeId })
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.CompanyId)
                .Index(t => t.ScopeId);

            Sql(@"insert dbo.CompanyXScope(CompanyId,ScopeId)" + System.Environment.NewLine +
                @"select CompanyProfile_Id,Scope_Id from ScopeCompanyProfile");

            CreateTable(
                "dbo.UserXScope",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ScopeId })
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.UserId)
                .Index(t => t.ScopeId);

            Sql(@"insert dbo.UserXScope(UserId,ScopeId)" + System.Environment.NewLine +
                @"select UserProfile_UserId,Scope_Id from ScopeUserProfile");
            
            CreateTable(
                "dbo.ProjectXScope",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.ScopeId })
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.ProjectId)
                .Index(t => t.ScopeId);
            
            DropTable("dbo.ScopeUserProfile");
            DropTable("dbo.ScopeCompanyProfile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ScopeCompanyProfile",
                c => new
                    {
                        Scope_Id = c.Int(nullable: false),
                        CompanyProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.CompanyProfile_Id });

            Sql(@"insert dbo.ScopeCompanyProfile(Scope_Id,CompanyProfile_Id)" + System.Environment.NewLine + 
                @"select ScopeId,CompanyId, from CompanyXScope");

            CreateTable(
                "dbo.ScopeUserProfile",
                c => new
                    {
                        Scope_Id = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.UserProfile_UserId });
            Sql(@"insert dbo.ScopeUserProfile(Scope_Id,UserProfile_UserId)" + System.Environment.NewLine +
                @"select ScopeId,UserId from UserXScope");
            
            DropIndex("dbo.ProjectXScope", new[] { "ScopeId" });
            DropIndex("dbo.ProjectXScope", new[] { "ProjectId" });
            DropIndex("dbo.UserXScope", new[] { "ScopeId" });
            DropIndex("dbo.UserXScope", new[] { "UserId" });
            DropIndex("dbo.CompanyXScope", new[] { "ScopeId" });
            DropIndex("dbo.CompanyXScope", new[] { "CompanyId" });
            DropForeignKey("dbo.ProjectXScope", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.ProjectXScope", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.UserXScope", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.UserXScope", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.CompanyXScope", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.CompanyXScope", "CompanyId", "dbo.CompanyProfile");
            DropTable("dbo.ProjectXScope");
            DropTable("dbo.UserXScope");
            DropTable("dbo.CompanyXScope");
            CreateIndex("dbo.ScopeCompanyProfile", "CompanyProfile_Id");
            CreateIndex("dbo.ScopeCompanyProfile", "Scope_Id");
            CreateIndex("dbo.ScopeUserProfile", "UserProfile_UserId");
            CreateIndex("dbo.ScopeUserProfile", "Scope_Id");
            AddForeignKey("dbo.ScopeCompanyProfile", "CompanyProfile_Id", "dbo.CompanyProfile", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScopeCompanyProfile", "Scope_Id", "dbo.Scope", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScopeUserProfile", "UserProfile_UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.ScopeUserProfile", "Scope_Id", "dbo.Scope", "Id", cascadeDelete: true);
        }
    }
}
