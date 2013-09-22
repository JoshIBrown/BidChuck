namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOwnerToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "ClientId", c => c.Int());
            AddColumn("dbo.Project", "CompanyProfile_Id", c => c.Int());
            AddForeignKey("dbo.Project", "ClientId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.Project", "CompanyProfile_Id", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.Project", "ClientId");
            CreateIndex("dbo.Project", "CompanyProfile_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Project", new[] { "CompanyProfile_Id" });
            DropIndex("dbo.Project", new[] { "ClientId" });
            DropForeignKey("dbo.Project", "CompanyProfile_Id", "dbo.CompanyProfile");
            DropForeignKey("dbo.Project", "ClientId", "dbo.CompanyProfile");
            DropColumn("dbo.Project", "CompanyProfile_Id");
            DropColumn("dbo.Project", "ClientId");
        }
    }
}
