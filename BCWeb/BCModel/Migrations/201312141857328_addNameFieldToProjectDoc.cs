namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameFieldToProjectDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDocument", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectDocument", "Name");
        }
    }
}
