namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIpAddressToAuditTrail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBAudit", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DBAudit", "Address");
        }
    }
}
