namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeBusinessAndStateToNotNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "CountyId", "dbo.County");
            DropForeignKey("dbo.UserProfile", "StateId", "dbo.State");
            DropIndex("dbo.UserProfile", new[] { "CountyId" });
            DropIndex("dbo.UserProfile", new[] { "StateId" });
            AddColumn("dbo.UserProfile", "JobTitle", c => c.String());
            DropIndex("dbo.CompanyProfile", new[] { "BusinessTypeId" });
            DropIndex("dbo.CompanyProfile", new[] { "StateId" });
            AlterColumn("dbo.CompanyProfile", "StateId", c => c.Int(nullable: false));
            AlterColumn("dbo.CompanyProfile", "BusinessTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.CompanyProfile", "StateId");
            CreateIndex("dbo.CompanyProfile", "BusinessTypeId");
            DropColumn("dbo.UserProfile", "County_Id");
            DropColumn("dbo.UserProfile", "State_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "State_Id", c => c.Int());
            AddColumn("dbo.UserProfile", "County_Id", c => c.Int());
            AlterColumn("dbo.CompanyProfile", "BusinessTypeId", c => c.Int());
            AlterColumn("dbo.CompanyProfile", "StateId", c => c.Int());
            DropColumn("dbo.UserProfile", "JobTitle");
            CreateIndex("dbo.UserProfile", "State_Id");
            CreateIndex("dbo.UserProfile", "County_Id");
            AddForeignKey("dbo.UserProfile", "State_Id", "dbo.State", "Id");
            AddForeignKey("dbo.UserProfile", "County_Id", "dbo.County", "Id");
        }
    }
}
