namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createManyToManyScopeXCompany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scope", "CompanyProfile_Id", "dbo.CompanyProfile");
            DropIndex("dbo.Scope", new[] { "CompanyProfile_Id" });
            CreateTable(
                "dbo.ScopeCompanyProfile",
                c => new
                    {
                        Scope_Id = c.Int(nullable: false),
                        CompanyProfile_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.CompanyProfile_Id })
                .ForeignKey("dbo.Scope", t => t.Scope_Id, cascadeDelete: true)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyProfile_Id, cascadeDelete: true)
                .Index(t => t.Scope_Id)
                .Index(t => t.CompanyProfile_Id);
            
            DropColumn("dbo.Scope", "CompanyProfile_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scope", "CompanyProfile_Id", c => c.Int());
            DropIndex("dbo.ScopeCompanyProfile", new[] { "CompanyProfile_Id" });
            DropIndex("dbo.ScopeCompanyProfile", new[] { "Scope_Id" });
            DropForeignKey("dbo.ScopeCompanyProfile", "CompanyProfile_Id", "dbo.CompanyProfile");
            DropForeignKey("dbo.ScopeCompanyProfile", "Scope_Id", "dbo.Scope");
            DropTable("dbo.ScopeCompanyProfile");
            CreateIndex("dbo.Scope", "CompanyProfile_Id");
            AddForeignKey("dbo.Scope", "CompanyProfile_Id", "dbo.CompanyProfile", "Id");
        }
    }
}
