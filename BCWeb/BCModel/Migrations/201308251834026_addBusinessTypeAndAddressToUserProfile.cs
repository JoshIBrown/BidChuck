namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBusinessTypeAndAddressToUserProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserProfile", "Address1", c => c.String());
            AddColumn("dbo.UserProfile", "Address2", c => c.String());
            AddColumn("dbo.UserProfile", "City", c => c.String());
            AddColumn("dbo.UserProfile", "PostalCode", c => c.String(nullable: false));
            AddColumn("dbo.UserProfile", "OperatingDistance", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "BusinessTypeId", c => c.Int());
            AddForeignKey("dbo.UserProfile", "BusinessTypeId", "dbo.BusinessType", "Id");
            CreateIndex("dbo.UserProfile", "BusinessTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserProfile", new[] { "BusinessTypeId" });
            DropForeignKey("dbo.UserProfile", "BusinessTypeId", "dbo.BusinessType");
            DropColumn("dbo.UserProfile", "BusinessTypeId");
            DropColumn("dbo.UserProfile", "OperatingDistance");
            DropColumn("dbo.UserProfile", "PostalCode");
            DropColumn("dbo.UserProfile", "City");
            DropColumn("dbo.UserProfile", "Address2");
            DropColumn("dbo.UserProfile", "Address1");
            DropTable("dbo.BusinessType");
        }
    }
}
