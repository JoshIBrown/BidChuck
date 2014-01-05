namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessageToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Message");
        }
    }
}
