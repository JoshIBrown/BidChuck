namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeNumberToCsiNumber : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Scope", "Number", "CsiNumber");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Scope", "CsiNumber", "Number");
        }
    }
}
