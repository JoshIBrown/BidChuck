namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buildingTypesMutliTier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BuildingType", "ParentId", c => c.Int());
            AddForeignKey("dbo.BuildingType", "ParentId", "dbo.BuildingType", "Id");
            CreateIndex("dbo.BuildingType", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BuildingType", new[] { "ParentId" });
            DropForeignKey("dbo.BuildingType", "ParentId", "dbo.BuildingType");
            DropColumn("dbo.BuildingType", "ParentId");
        }
    }
}
