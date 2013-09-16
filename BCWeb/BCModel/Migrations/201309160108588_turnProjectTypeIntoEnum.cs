namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class turnProjectTypeIntoEnum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType");
            DropIndex("dbo.Project", new[] { "ProjectTypeId" });
            AddColumn("dbo.Project", "ProjectType", c => c.Int(nullable: false));
            DropColumn("dbo.Project", "ProjectTypeId");
            DropTable("dbo.ProjectType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Project", "ProjectTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Project", "ProjectType");
            CreateIndex("dbo.Project", "ProjectTypeId");
            AddForeignKey("dbo.Project", "ProjectTypeId", "dbo.ProjectType", "Id");
        }
    }
}
