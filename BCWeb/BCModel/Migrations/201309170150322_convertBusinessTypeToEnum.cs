namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class convertBusinessTypeToEnum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyProfile", "BusinessTypeId", "dbo.BusinessType");
            DropIndex("dbo.CompanyProfile", new[] { "BusinessTypeId" });
            AddColumn("dbo.CompanyProfile", "BusinessType", c => c.Int(nullable: false));
            Sql(@"Update c
                set c.BusinessType = case b.Name
                    when 'General Contractor' then 0
                    when 'Sub-Contractor' then 1
                    when 'Architect' then 2
                    when 'Engineer' then 6
                    when 'Owner/Client' then 7
                    when 'Materials Vendor' then 3
                    when 'Materials Manufacturer' then 4
                    when 'Consultant' then 5
                    else -1 end
                from dbo.CompanyProfile c
                join dbo.BusinessType b
                on c.BusinessTypeId = b.Id
            ");

            DropColumn("dbo.CompanyProfile", "BusinessTypeId");
            DropTable("dbo.BusinessType");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.BusinessType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.CompanyProfile", "BusinessTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.CompanyProfile", "BusinessType");
            CreateIndex("dbo.CompanyProfile", "BusinessTypeId");
            AddForeignKey("dbo.CompanyProfile", "BusinessTypeId", "dbo.BusinessType", "Id");
        }
    }
}
