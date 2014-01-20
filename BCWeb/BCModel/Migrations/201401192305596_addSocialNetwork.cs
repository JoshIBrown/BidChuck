namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSocialNetwork : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NetworkConnection",
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyProfile", t => t.SenderId)
                .ForeignKey("dbo.CompanyProfile", t => t.RecipientId)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.NetworkRequest", new[] { "RecipientId" });
            DropIndex("dbo.NetworkRequest", new[] { "SenderId" });
            DropIndex("dbo.NetworkConnection", new[] { "RightId" });
            DropIndex("dbo.NetworkConnection", new[] { "LeftId" });
            DropForeignKey("dbo.NetworkRequest", "RecipientId", "dbo.CompanyProfile");
            DropForeignKey("dbo.NetworkRequest", "SenderId", "dbo.CompanyProfile");
            DropForeignKey("dbo.NetworkConnection", "RightId", "dbo.CompanyProfile");
            DropForeignKey("dbo.NetworkConnection", "LeftId", "dbo.CompanyProfile");
            DropTable("dbo.NetworkRequest");
            DropTable("dbo.NetworkConnection");
        }
    }
}
