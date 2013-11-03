namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addProjectCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "ProjectCategory", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.Project", "ProjectCategory");
        }
    }
}
