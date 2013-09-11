namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moreMetadataForProjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "BidDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Project", "Address1", c => c.String());
            AddColumn("dbo.Project", "Address2", c => c.String());
            AddColumn("dbo.Project", "City", c => c.String());
            AddColumn("dbo.Project", "PostalCode", c => c.String());
            AddColumn("dbo.Project", "StateId", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "Architect", c => c.String());
            AddForeignKey("dbo.Project", "StateId", "dbo.State", "Id");
            CreateIndex("dbo.Project", "StateId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Project", new[] { "StateId" });
            DropForeignKey("dbo.Project", "StateId", "dbo.State");
            DropColumn("dbo.Project", "Architect");
            DropColumn("dbo.Project", "StateId");
            DropColumn("dbo.Project", "PostalCode");
            DropColumn("dbo.Project", "City");
            DropColumn("dbo.Project", "Address2");
            DropColumn("dbo.Project", "Address1");
            DropColumn("dbo.Project", "BidDateTime");
        }
    }
}
