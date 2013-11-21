namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHierarchyToConstructionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConstructionType", "ParentId", c => c.Int());
            AddForeignKey("dbo.ConstructionType", "ParentId", "dbo.ConstructionType", "Id");
            CreateIndex("dbo.ConstructionType", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ConstructionType", new[] { "ParentId" });
            DropForeignKey("dbo.ConstructionType", "ParentId", "dbo.ConstructionType");
            DropColumn("dbo.ConstructionType", "ParentId");
        }
    }
}
