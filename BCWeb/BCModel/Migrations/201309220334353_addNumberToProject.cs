namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNumberToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Number", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Number");
        }
    }
}
