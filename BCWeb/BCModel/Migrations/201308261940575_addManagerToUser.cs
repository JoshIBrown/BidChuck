namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addManagerToUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropIndex("dbo.UserProfile", new[] { "UserProfile_UserId" });
            AddColumn("dbo.UserProfile", "ManagerId", c => c.Int(nullable: true));
            AddForeignKey("dbo.UserProfile", "ManagerId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.UserProfile", "ManagerId");
            Sql("update dbo.UserProfile set ManagerId = UserProfile_UserId");
            DropColumn("dbo.UserProfile", "UserProfile_UserId");
        }

        public override void Down()
        {
            AddColumn("dbo.UserProfile", "UserProfile_UserId", c => c.Int());
            DropIndex("dbo.UserProfile", new[] { "ManagerId" });
            DropForeignKey("dbo.UserProfile", "ManagerId", "dbo.UserProfile");
            Sql("update dbo.UserProfile set UserProfile_UserId = ManagerId");
            DropColumn("dbo.UserProfile", "ManagerId");
            CreateIndex("dbo.UserProfile", "UserProfile_UserId");
            AddForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
