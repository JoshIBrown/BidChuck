namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeFieldsInProjectRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Project", "PostalCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "PostalCode", c => c.String());
            AlterColumn("dbo.Project", "City", c => c.String());
        }
    }
}
