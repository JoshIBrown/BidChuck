namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAddressFieldToAuditTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBAudit", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DBAudit", "Address");
        }
    }
}
