namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInvitationType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidPackageXInvitee", "InvitationType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BidPackageXInvitee", "InvitationType");
        }
    }
}
