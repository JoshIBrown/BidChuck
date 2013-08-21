namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foo : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.UserProfile", "FirstName", c => c.String());
            //AlterColumn("dbo.UserProfile", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.UserProfile", "LastName", c => c.String(nullable: false));
            //AlterColumn("dbo.UserProfile", "FirstName", c => c.String(nullable: false));
        }
    }
}
