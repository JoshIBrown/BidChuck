namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class renameSocialNetwork : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notification", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.NetworkRequest", "SenderId", "dbo.CompanyProfile");
            DropForeignKey("dbo.NetworkRequest", "RecipientId", "dbo.CompanyProfile");
            DropIndex("dbo.Notification", new[] { "ProjectId" });
            DropIndex("dbo.NetworkRequest", new[] { "SenderId" });
            DropIndex("dbo.NetworkRequest", new[] { "RecipientId" });
            CreateTable(
                "dbo.ConnectionRequest",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        AcceptDate = c.DateTime(),
                        DeclineDate = c.DateTime(),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyProfile", t => t.SenderId)
                .ForeignKey("dbo.CompanyProfile", t => t.RecipientId)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);

            Sql(@"  
                    insert dbo.ConnectionRequest(Id, SenderId,RecipientId,AcceptDate,DeclineDate,SentDate)
                    select * from dbo.NetworkRequest
                    ");

            AddColumn("dbo.Notification", "EntityId", c => c.Int(nullable: false));
            AddColumn("dbo.Notification", "EntityType", c => c.Int(nullable: false));
            AddColumn("dbo.CompanyProfile", "CustomUrl", c => c.String());
            Sql(@" Update Notification
                   Set EntityId = ProjectId,
                   EntityType = case when NotificationType in (0,1,2,3,4) then 0 when NotificationType in (5,6) then 1 else 0 end              
                ");
            DropColumn("dbo.Notification", "ProjectId");
            DropTable("dbo.NetworkRequest");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.NetworkRequest",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        AcceptDate = c.DateTime(),
                        DeclineDate = c.DateTime(),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Notification", "ProjectId", c => c.Int(nullable: false));
            DropIndex("dbo.ConnectionRequest", new[] { "RecipientId" });
            DropIndex("dbo.ConnectionRequest", new[] { "SenderId" });
            DropForeignKey("dbo.ConnectionRequest", "RecipientId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ConnectionRequest", "SenderId", "dbo.CompanyProfile");
            DropColumn("dbo.CompanyProfile", "CustomUrl");
            DropColumn("dbo.Notification", "EntityType");
            DropColumn("dbo.Notification", "EntityId");
            DropTable("dbo.ConnectionRequest");
            CreateIndex("dbo.NetworkRequest", "RecipientId");
            CreateIndex("dbo.NetworkRequest", "SenderId");
            CreateIndex("dbo.Notification", "ProjectId");
            AddForeignKey("dbo.NetworkRequest", "RecipientId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.NetworkRequest", "SenderId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.Notification", "ProjectId", "dbo.Project", "Id");
        }
    }
}
