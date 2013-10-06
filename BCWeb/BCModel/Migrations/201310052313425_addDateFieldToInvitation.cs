namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateFieldToInvitation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackageXInvitee", "Accepted", c => c.DateTime(nullable: false));
            AddColumn("dbo.BidPackageXInvitee", "Rejected", c => c.DateTime(nullable: false));
            DropColumn("dbo.BidPackageXInvitee", "InviteStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BidPackageXInvitee", "InviteStatus", c => c.Int(nullable: false));
            DropColumn("dbo.BidPackageXInvitee", "Rejected");
            DropColumn("dbo.BidPackageXInvitee", "Accepted");
        }
    }
}
