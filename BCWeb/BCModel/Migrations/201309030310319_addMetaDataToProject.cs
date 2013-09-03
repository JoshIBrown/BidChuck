namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMetaDataToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "BuildingTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "ProjectTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "ConstructionTypeId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Project", "BuildingTypeId", "dbo.BuildingType", "Id");
            AddForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType", "Id");
            AddForeignKey("dbo.Project", "ConstructionTypeId", "dbo.ConstructionType", "Id");
            CreateIndex("dbo.Project", "BuildingTypeId");
            CreateIndex("dbo.Project", "ProjectTypeId");
            CreateIndex("dbo.Project", "ConstructionTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Project", new[] { "ConstructionTypeId" });
            DropIndex("dbo.Project", new[] { "ProjectTypeId" });
            DropIndex("dbo.Project", new[] { "BuildingTypeId" });
            DropForeignKey("dbo.Project", "ConstructionTypeId", "dbo.ConstructionType");
            DropForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType");
            DropForeignKey("dbo.Project", "BuildingTypeId", "dbo.BuildingType");
            DropColumn("dbo.Project", "ConstructionTypeId");
            DropColumn("dbo.Project", "ProjectTypeId");
            DropColumn("dbo.Project", "BuildingTypeId");
        }
    }
}
