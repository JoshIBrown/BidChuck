namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeStateNullableInCompanyProfile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyProfile", "StateId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyProfile", "StateId", c => c.Int(nullable: false));
        }
    }
}
