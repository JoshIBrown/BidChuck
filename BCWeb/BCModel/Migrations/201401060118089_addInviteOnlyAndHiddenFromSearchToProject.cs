namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInviteOnlyAndHiddenFromSearchToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "InvitationOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "HiddenFromSearch", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "HiddenFromSearch");
            DropColumn("dbo.Project", "InvitationOnly");
        }
    }
}
