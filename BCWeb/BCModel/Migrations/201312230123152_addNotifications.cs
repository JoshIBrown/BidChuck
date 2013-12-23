namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "LastEditTimestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "LastEditTimestamp");
        }
    }
}
