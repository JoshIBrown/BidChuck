namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotesToBlackList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NetworkConnection", "LeftId", "dbo.CompanyProfile");
            DropForeignKey("dbo.NetworkConnection", "RightId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ConnectionRequest", "SenderId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ConnectionRequest", "RecipientId", "dbo.CompanyProfile");
            DropIndex("dbo.NetworkConnection", new[] { "LeftId" });
            DropIndex("dbo.NetworkConnection", new[] { "RightId" });
            DropIndex("dbo.ConnectionRequest", new[] { "SenderId" });
            DropIndex("dbo.ConnectionRequest", new[] { "RecipientId" });
            CreateTable(
                "dbo.ContactConnection",
                c => new
                    {
                        LeftId = c.Int(nullable: false),
                        RightId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.LeftId, t.RightId })
                .ForeignKey("dbo.CompanyProfile", t => t.LeftId)
                .ForeignKey("dbo.CompanyProfile", t => t.RightId)
                .Index(t => t.LeftId)
                .Index(t => t.RightId);

            Sql(@"insert ContactConnection(LeftId,RightId,CreateDate)
                select * from dbo.NetworkConnection");
            
            CreateTable(
                "dbo.ContactRequest",
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

            Sql(@"insert ContactRequest(id, senderid, recipientid,acceptdate,declinedate,sentdate)
                select * from ConnectionRequest");
            
            AddColumn("dbo.BlackList", "Notes", c => c.String());
            DropTable("dbo.NetworkConnection");
            DropTable("dbo.ConnectionRequest");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);

            Sql(@"insert ConnectionRequest (id, senderid, recipientid,acceptdate,declinedate,sentdate)
                select * from ContactRequest");
            
            CreateTable(
                "dbo.NetworkConnection",
                c => new
                    {
                        LeftId = c.Int(nullable: false),
                        RightId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.LeftId, t.RightId });

            Sql(@"insert NetworkConnection(LeftId,RightId,CreateDate)
                select * from ContactConnection");
            
            DropIndex("dbo.ContactRequest", new[] { "RecipientId" });
            DropIndex("dbo.ContactRequest", new[] { "SenderId" });
            DropIndex("dbo.ContactConnection", new[] { "RightId" });
            DropIndex("dbo.ContactConnection", new[] { "LeftId" });
            DropForeignKey("dbo.ContactRequest", "RecipientId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ContactRequest", "SenderId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ContactConnection", "RightId", "dbo.CompanyProfile");
            DropForeignKey("dbo.ContactConnection", "LeftId", "dbo.CompanyProfile");
            DropColumn("dbo.BlackList", "Notes");
            DropTable("dbo.ContactRequest");
            DropTable("dbo.ContactConnection");
            CreateIndex("dbo.ConnectionRequest", "RecipientId");
            CreateIndex("dbo.ConnectionRequest", "SenderId");
            CreateIndex("dbo.NetworkConnection", "RightId");
            CreateIndex("dbo.NetworkConnection", "LeftId");
            AddForeignKey("dbo.ConnectionRequest", "RecipientId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.ConnectionRequest", "SenderId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.NetworkConnection", "RightId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.NetworkConnection", "LeftId", "dbo.CompanyProfile", "Id");
        }
    }
}
