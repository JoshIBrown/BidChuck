namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addBaseAndComputedBid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseBid",
                c => new
                    {
                        BidId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.BidId, t.ScopeId })
                .ForeignKey("dbo.Bid", t => t.BidId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.BidId)
                .Index(t => t.ScopeId);

            CreateTable(
                "dbo.ComputedBid",
                c => new
                    {
                        BidId = c.Int(nullable: false),
                        ScopeId = c.Int(nullable: false),
                        RiskFactor = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.BidId, t.ScopeId })
                .ForeignKey("dbo.Bid", t => t.BidId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.BidId)
                .Index(t => t.ScopeId);
        }

        public override void Down()
        {
            DropIndex("dbo.ComputedBid", new[] { "ScopeId" });
            DropIndex("dbo.ComputedBid", new[] { "BidId" });
            DropIndex("dbo.BaseBid", new[] { "ScopeId" });
            DropIndex("dbo.BaseBid", new[] { "BidId" });
            DropForeignKey("dbo.ComputedBid", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.ComputedBid", "BidId", "dbo.Bid");
            DropForeignKey("dbo.BaseBid", "ScopeId", "dbo.Scope");
            DropForeignKey("dbo.BaseBid", "BidId", "dbo.Bid");
            DropTable("dbo.ComputedBid");
            DropTable("dbo.BaseBid");
        }
    }
}
