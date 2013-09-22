namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMetadataToCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyProfile", "BusinessLicense", c => c.String());
            AddColumn("dbo.CompanyProfile", "Website", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyProfile", "Website");
            DropColumn("dbo.CompanyProfile", "BusinessLicense");
        }
    }
}
