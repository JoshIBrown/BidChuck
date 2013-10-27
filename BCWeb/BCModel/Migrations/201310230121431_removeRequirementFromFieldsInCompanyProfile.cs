namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequirementFromFieldsInCompanyProfile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyProfile", "PostalCode", c => c.String());
            AlterColumn("dbo.CompanyProfile", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyProfile", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.CompanyProfile", "PostalCode", c => c.String(nullable: false));
        }
    }
}
