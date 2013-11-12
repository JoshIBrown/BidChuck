namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class refactorBidPackageInvitation : DbMigration
    {
        public override void Up()
        {

            DropForeignKey("dbo.BidPackageXInvitee", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.BidPackageXInvitee", "CompanyId", "dbo.CompanyProfile");
            DropIndex("dbo.BidPackageXInvitee", new[] { "BidPackageId" });
            DropIndex("dbo.BidPackageXInvitee", new[] { "CompanyId" });
            AlterColumn("dbo.BidPackageXInvitee", "CompanyId", c => c.Int(nullable: false));
            CreateTable(
                "dbo.Invitation",
                c => new
                    {
                        BidPackageId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                        AcceptedDate = c.DateTime(),
                        RejectedDate = c.DateTime(),
                        InvitationType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BidPackageId, t.CompanyId })
                .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .Index(t => t.BidPackageId)
                .Index(t => t.CompanyId);

            Sql(@"insert dbo.Invitation(BidPackageId,CompanyId,SentDate,AcceptedDate,RejectedDate,InvitationType)
                  select BidPackageId,CompanyId,SentDate,AcceptedDate,RejectedDate,InvitationType from dbo.BidPackageXInvitee");

            DropTable("dbo.BidPackageXInvitee");

        }

        public override void Down()
        {
            CreateTable(
                "dbo.BidPackageXInvitee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BidPackageId = c.Int(nullable: false),
                        CompanyId = c.Int(),
                        Email = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        AcceptedDate = c.DateTime(),
                        RejectedDate = c.DateTime(),
                        InvitationType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            Sql(@"insert dbo.BidPackageXInvitee(BidPackageId,CompanyId,SentDate,AcceptedDate,RejectedDate,InvitationType)
                  select BidPackageId,CompanyId,SentDate,AcceptedDate,RejectedDate,InvitationType from dbo.Invitation");
            DropIndex("dbo.Invitation", new[] { "CompanyId" });
            DropIndex("dbo.Invitation", new[] { "BidPackageId" });
            DropForeignKey("dbo.Invitation", "CompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.Invitation", "BidPackageId", "dbo.BidPackage");
            DropTable("dbo.Invitation");

        }
    }
}
