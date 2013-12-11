namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotesToProjectDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDocument", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectDocument", "Notes");
        }
    }
}
