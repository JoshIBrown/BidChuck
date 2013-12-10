namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addWalkthroughToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "WalkThruDateTime", c => c.DateTime());
            AddColumn("dbo.Project", "NoWalkThru", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Project", "WalkThruTBD", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Project", "WalkThruTBD");
            DropColumn("dbo.Project", "NoWalkThru");
            DropColumn("dbo.Project", "WalkThruDateTime");
        }
    }
}
