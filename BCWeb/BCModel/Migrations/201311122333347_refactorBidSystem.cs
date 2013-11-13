namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class refactorBidSystem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.Bid", "BiddingCompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.BaseBid", "BidId", "dbo.Bid");
            DropForeignKey("dbo.ComputedBid", "BidId", "dbo.Bid");
            DropForeignKey("dbo.Invitation", "CompanyId", "dbo.CompanyProfile");
            DropIndex("dbo.Bid", new[] { "BidPackageId" });
            DropIndex("dbo.Bid", new[] { "BiddingCompanyId" });
            DropIndex("dbo.BaseBid", new[] { "BidId" });
            DropIndex("dbo.ComputedBid", new[] { "BidId" });
            DropIndex("dbo.Invitation", new[] { "CompanyId" });

            // rename tables to old. have to do this because
            // we are changing the primary keys or the pk name.
            // procedure is to make a new table with updated schema, 
            // copy over data, and then drop the old table
            RenameTable("BaseBid", "BaseBidOld");
            RenameTable("ComputedBid", "ComputedBidOld");



            RenameColumn("dbo.Invitation", "CompanyId", "SentToId");
            AddForeignKey("dbo.Invitation", "SentToId", "dbo.CompanyProfile");
            CreateIndex("dbo.Invitation", "SentToId");

            //            Sql(@"insert dbo.Invitation(BidPackageId,SentToId,SentDate,AcceptedDate,RejectedDate,InvitationType)
            //                  select BidPackageId,CompanyId,SentDate,AcceptedDate,RejectedDate,InvitationType from dbo.InvitationOld");

            CreateTable("ComputedBid", c => new
            {
                BidPackageId = c.Int(nullable: false),
                SentToId = c.Int(nullable: false),
                ScopeId = c.Int(nullable: false),
                RiskFactor = c.Decimal(nullable: true, precision: 10, scale: 8)
            })
            .PrimaryKey(t => new { t.BidPackageId, t.SentToId, t.ScopeId })
            .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
            .ForeignKey("dbo.CompanyProfile", t => t.SentToId)
            .ForeignKey("dbo.Scope", t => t.ScopeId)
            .Index(t => t.BidPackageId)
            .Index(t => t.SentToId)
            .Index(t => t.ScopeId);

            Sql(@"insert dbo.ComputedBid(BidPackageId,SentToId,ScopeId,RiskFactor)
                  select bb.BidPackageId, bb.BiddingCompanyId,cb.ScopeId,cb.RiskFactor from dbo.computedbidold cb ,bid bb
                  where cb.bidid = bb.id ");

            CreateTable("BaseBid", c => new
            {
                ProjectId = c.Int(nullable: false),
                SentToId = c.Int(nullable: false),
                ScopeId = c.Int(nullable: false),
                Amount = c.Decimal(nullable: false, precision: 24, scale: 4)
            })
            .PrimaryKey(t => new { t.ProjectId, t.SentToId, t.ScopeId })
            .ForeignKey("dbo.Project", t => t.ProjectId)
            .ForeignKey("dbo.CompanyProfile", t => t.SentToId)
            .ForeignKey("dbo.Scope", t => t.ScopeId)
            .Index(t => t.ScopeId)
            .Index(t => t.ProjectId)
            .Index(t => t.SentToId);

            Sql(@"insert dbo.BaseBid(ProjectId,SentToId,ScopeId,Amount)
                  select bp.ProjectId,b.BiddingCompanyId,bb.ScopeId,bb.Amount from basebidold bb,bid b,bidpackage bp
                  where bb.bidid = b.id and b.BidPackageId = bp.id");



            DropTable("BaseBidOld");
            DropTable("ComputedBidOld");

            DropTable("dbo.Bid");
        }

        public override void Down()
        {
            DropIndex("dbo.BaseBid", new[] { "SentToId" });
            DropIndex("dbo.BaseBid", new[] { "ProjectId" });
            DropIndex("dbo.ComputedBid", new[] { "SentToId" });
            DropIndex("dbo.ComputedBid", new[] { "BidPackageId" });
            DropIndex("dbo.Invitation", new[] { "SentToId" });
            //DropForeignKey("dbo.BaseBid", "SentToId", "dbo.CompanyProfile");
            //DropForeignKey("dbo.BaseBid", "ProjectId", "dbo.Project");
            //DropForeignKey("dbo.ComputedBid", "SentToId", "dbo.CompanyProfile");
            //DropForeignKey("dbo.ComputedBid", "BidPackageId", "dbo.BidPackage");
            DropForeignKey("dbo.Invitation", "SentToId", "dbo.CompanyProfile");
            AlterColumn("dbo.BaseBid", "Amount", c => c.Decimal(precision: 18, scale: 2));

            RenameTable("BaseBid", "BaseBidOld");
            RenameTable("ComputedBid", "ComputedBidOld");


            CreateTable("dbo.Bid", c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BidPackageId = c.Int(nullable: false),
                        BiddingCompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
                .ForeignKey("dbo.CompanyProfile", t => t.BiddingCompanyId)
                .Index(t => t.BidPackageId)
                .Index(t => t.BiddingCompanyId);

            Sql(@"insert Bid(BidPackageId,BiddingCompanyId)
                  Select distinct BidPackageId,SentToId from ComputedBidOld");



            CreateTable("dbo.BaseBid", c => new
                {
                    BidId = c.Int(nullable: false),
                    ScopeId = c.Int(nullable: false),
                    Amount = c.Decimal(precision: 18, scale: 2)
                })
                .PrimaryKey(t => new { t.BidId, t.ScopeId })
                .ForeignKey("dbo.Bid", t => t.BidId)
                .ForeignKey("dbo.Scope", t => t.ScopeId)
                .Index(t => t.BidId)
                .Index(t => t.ScopeId);

            Sql(@"insert dbo.BaseBid(BidId,ScopeId,Amount)
                  Select b.Id, bb.ScopeId,bb.Amount from dbo.Bid b, BidPackage bp, dbo.BaseBidOld bb, dbo.Invitation i
                  where b.BidPackageId = bp.Id 
                  and bb.ProjectId = bp.ProjectId 
                  and i.SentToId = b.BiddingCompanyId 
                  and i.BidPackageId = bp.Id");

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

            Sql(@"insert ComputedBid(BidId,ScopeId,RiskFactor)
                  Select b.Id,cb.ScopeId,cb.RiskFactor
                  From ComputedBidOld cb,Bid b
                  Where cb.BidPackageId = b.BidPackageId and cb.SentToId = b.BiddingCompanyId");

            RenameColumn("dbo.Invitation", "SentToId", "CompanyId");
            AddForeignKey("dbo.Invitation", "CompanyId", "dbo.CompanyProfile");
            CreateIndex("dbo.Invitation", "CompanyId");

            DropTable("BaseBidOld");
            DropTable("ComputedBidOld");

        }
    }
}
