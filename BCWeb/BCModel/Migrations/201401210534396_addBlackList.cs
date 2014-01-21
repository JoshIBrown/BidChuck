namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBlackList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlackList",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        BlackListedCompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.BlackListedCompanyId })
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .ForeignKey("dbo.CompanyProfile", t => t.BlackListedCompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.BlackListedCompanyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BlackList", new[] { "BlackListedCompanyId" });
            DropIndex("dbo.BlackList", new[] { "CompanyId" });
            DropForeignKey("dbo.BlackList", "BlackListedCompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.BlackList", "CompanyId", "dbo.CompanyProfile");
            DropTable("dbo.BlackList");
        }
    }
}
