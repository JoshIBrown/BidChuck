namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class makeAcceptRejectInvitationNullable : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.BidPackageXInvitee", "Sent", "SentDate");
            RenameColumn("dbo.BidPackageXInvitee", "Accepted", "AcceptedDate");
            RenameColumn("dbo.BidPackageXInvitee", "Rejected", "RejectedDate");
            AlterColumn("dbo.BidPackageXInvitee", "AcceptedDate", c => c.DateTime(nullable: true));
            AlterColumn("dbo.BidPackageXInvitee","RejectedDate", c => c.DateTime(nullable: true));
        }

        public override void Down()
        {
            RenameColumn("dbo.BidPackageXInvitee", "RejectedDate", "Rejected");
            RenameColumn("dbo.BidPackageXInvitee", "AcceptedDate", "Accepted");
            RenameColumn("dbo.BidPackageXInvitee", "SentDate", "Sent");
        }
    }
}
