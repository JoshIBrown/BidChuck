namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStateAndCountyToUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Abbr = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.County",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId)
                .Index(t => t.StateId);
            
            AddColumn("dbo.UserProfile", "StateId", c => c.Int(nullable: true));
            AddColumn("dbo.UserProfile", "CountyId", c => c.Int(nullable: true));
            AddForeignKey("dbo.UserProfile", "CountyId", "dbo.County", "Id");
            AddForeignKey("dbo.UserProfile", "StateId", "dbo.State", "Id");
            CreateIndex("dbo.UserProfile", "CountyId");
            CreateIndex("dbo.UserProfile", "StateId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.County", new[] { "StateId" });
            DropIndex("dbo.UserProfile", new[] { "StateId" });
            DropIndex("dbo.UserProfile", new[] { "CountyId" });
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.UserProfile", "StateId", "dbo.State");
            DropForeignKey("dbo.UserProfile", "CountyId", "dbo.County");
            DropColumn("dbo.UserProfile", "CountyId");
            DropColumn("dbo.UserProfile", "StateId");
            DropTable("dbo.County");
            DropTable("dbo.State");
        }
    }
}
