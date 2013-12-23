namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeNotificationTemplatePrimaryKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.NotificationTemplate", new[] { "Id" });
            AddPrimaryKey("dbo.NotificationTemplate", "NotificationType");
            DropColumn("dbo.NotificationTemplate", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotificationTemplate", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.NotificationTemplate", new[] { "NotificationType" });
            AddPrimaryKey("dbo.NotificationTemplate", "Id");
        }
    }
}
