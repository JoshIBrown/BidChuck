namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNotificationTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Notification", "ProjectId", c => c.Int(nullable: false));
            AddColumn("dbo.Notification", "Count", c => c.Int(nullable: false));
            AddForeignKey("dbo.Notification", "ProjectId", "dbo.Project", "Id");
            CreateIndex("dbo.Notification", "ProjectId");
            DropColumn("dbo.Notification", "Description");
            DropColumn("dbo.Notification", "Url");
            DropColumn("dbo.Notification", "LinkText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "LinkText", c => c.String());
            AddColumn("dbo.Notification", "Url", c => c.String());
            AddColumn("dbo.Notification", "Description", c => c.String());
            DropIndex("dbo.Notification", new[] { "ProjectId" });
            DropForeignKey("dbo.Notification", "ProjectId", "dbo.Project");
            DropColumn("dbo.Notification", "Count");
            DropColumn("dbo.Notification", "ProjectId");
            DropTable("dbo.NotificationTemplate");
        }
    }
}
