namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeWalkThruStatusToEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "WalkThruStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Project", "NoWalkThru");
            DropColumn("dbo.Project", "WalkThruTBD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "WalkThruTBD", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "NoWalkThru", c => c.Boolean(nullable: false));
            DropColumn("dbo.Project", "WalkThruStatus");
        }
    }
}
