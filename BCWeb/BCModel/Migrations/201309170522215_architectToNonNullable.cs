namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class architectToNonNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Project", "IX_ArchitectId");
            AlterColumn("dbo.Project", "ArchitectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Project", "ArchitectId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Project", "IX_ArchitectId");
            AlterColumn("dbo.Project", "ArchitectId", c => c.Int());
            CreateIndex("dbo.Project", "ArchitextId");
        }
    }
}
