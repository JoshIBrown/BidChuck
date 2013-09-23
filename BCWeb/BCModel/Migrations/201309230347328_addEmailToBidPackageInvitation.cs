namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailToBidPackageInvitation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackageXInvitee", "Email", c => c.String());
            AddColumn("dbo.BidPackageXInvitee", "InviteStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.BidPackageXInvitee", "CompanyId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BidPackageXInvitee", "CompanyId", c => c.Int(nullable: false));
            DropColumn("dbo.BidPackageXInvitee", "InviteStatus");
            DropColumn("dbo.BidPackageXInvitee", "Email");
        }
    }
}
