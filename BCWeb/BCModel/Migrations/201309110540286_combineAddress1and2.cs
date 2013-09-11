namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class combineAddress1and2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Project", "Address1", "Address");
            DropColumn("dbo.Project", "Address2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "Address2", c => c.String());
            RenameColumn("dbo.Project", "Address", "Address1");
            
        }
    }
}
