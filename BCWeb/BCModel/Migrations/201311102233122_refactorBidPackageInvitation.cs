namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class refactorBidPackageInvitation : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.BidPackageXInvitee");
            DropForeignKey("dbo.BidPackageXInvitee", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.BidPackageXInvitee", "CompanyId", "dbo.CompanyProfile");
            DropIndex("dbo.BidPackageXInvitee", new[] { "BidPackageId" });
            DropIndex("dbo.BidPackageXInvitee", new[] { "CompanyId" });
            DropColumn("dbo.BidPackageXInvitee", "Id");
            AlterColumn("dbo.BidPackageXInvitee", "CompanyId", c => c.Int(nullable: false));
            //CreateTable(
            //    "dbo.Invitation",
            //    c => new
            //        {
            //            BidPackageId = c.Int(nullable: false),
            //            CompanyId = c.Int(nullable: false),
            //            SentDate = c.DateTime(nullable: false),
            //            AcceptedDate = c.DateTime(),
            //            RejectedDate = c.DateTime(),
            //            InvitationType = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.BidPackageId, t.CompanyId })
            //    .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
            //    .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
            //    .Index(t => t.BidPackageId)
            //    .Index(t => t.CompanyId);

            //DropTable("dbo.BidPackageXInvitee");
            RenameTable("BidPackageXInvitee", "Invitation");
            AddPrimaryKey("dbo.Invitation", new[] { "BidPackageId", "CompanyId" });
            AddForeignKey("dbo.Invitation", "BidPackageId", "dbo.BidPackage");
            AddForeignKey("dbo.Invitation", "CompanyId", "dbo.CompanyProfile");
            CreateIndex("dbo.Invitation", "BidPackageId");
            CreateIndex("dbo.Invitation", "CompanyId");
            CreateIndex("dbo.Invitation", new[] { "BidPackageId", "CompanyId" }, unique: true);
        }

        public override void Down()
        {
            //CreateTable(
            //    "dbo.BidPackageXInvitee",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            BidPackageId = c.Int(nullable: false),
            //            CompanyId = c.Int(),
            //            Email = c.String(),
            //            SentDate = c.DateTime(nullable: false),
            //            AcceptedDate = c.DateTime(),
            //            RejectedDate = c.DateTime(),
            //            InvitationType = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            DropIndex("dbo.Invitation", new[] { "CompanyId" });
            DropIndex("dbo.Invitation", new[] { "BidPackageId" });
            DropForeignKey("dbo.Invitation", "CompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.Invitation", "BidPackageId", "dbo.BidPackage");
            //DropTable("dbo.Invitation");
            RenameTable("Invitation", "BidPackageXInvitee");
            AlterColumn("dbo.BidPackageXInvitee", "CompanyId", c => c.Int(nullable: true));
            AddColumn("dbo.BidPackageXInvitee", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.BidPackageXInvitee", "Id");
            CreateIndex("dbo.BidPackageXInvitee", "CompanyId");
            CreateIndex("dbo.BidPackageXInvitee", "BidPackageId");
            AddForeignKey("dbo.BidPackageXInvitee", "CompanyId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.BidPackageXInvitee", "BidPackageId", "dbo.BidPackage", "Id");
        }
    }
}
