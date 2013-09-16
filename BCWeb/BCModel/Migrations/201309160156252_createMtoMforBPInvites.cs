namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class createMtoMforBPInvites : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "CreatorId", "dbo.UserProfile");
            DropIndex("dbo.Project", new[] { "CreatorId" });
            RenameColumn("dbo.Project", "CreatorId", "CreatedById");
            AddForeignKey("dbo.Project", "CreatedById", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.Project", "CreatedById");

            CreateTable(
                "dbo.BidPackageXInvitee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BidPackageId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Sent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BidPackage", t => t.BidPackageId)
                .ForeignKey("dbo.CompanyProfile", t => t.CompanyId)
                .Index(t => t.BidPackageId)
                .Index(t => t.CompanyId);

            AddColumn("dbo.Project", "ArchitectId", c => c.Int());

            AddColumn("dbo.BidPackage", "CreatorId", c => c.Int(nullable: false));


            AddForeignKey("dbo.BidPackage", "CreatorId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.Project", "ArchitectId", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.BidPackage", "CreatorId");
            CreateIndex("dbo.Project", "ArchitectId");
            DropColumn("dbo.Project", "Architect");
        }

        public override void Down()
        {
            AddColumn("dbo.Project", "Architect", c => c.String());
            DropForeignKey("dbo.Project", "CreatedById", "dbo.UserProfile");
            DropIndex("dbo.Project", new[] { "CreatedById" });
            RenameColumn("dbo.Project", "CreatedById", "CreatorId");
            CreateIndex("dbo.Project", "CreatorId");
            AddForeignKey("dbo.Project", "CreatorId", "dbo.UserProfile", "UserId");

            DropIndex("dbo.Project", new[] { "ArchitectId" });
            DropIndex("dbo.BidPackage", new[] { "CreatorId" });
            DropIndex("dbo.BidPackageXInvitee", new[] { "CompanyId" });
            DropIndex("dbo.BidPackageXInvitee", new[] { "BidPackageId" });
            DropForeignKey("dbo.Project", "ArchitectId", "dbo.CompanyProfile");

            DropForeignKey("dbo.BidPackage", "CreatorId", "dbo.CompanyProfile");
            DropForeignKey("dbo.BidPackageXInvitee", "CompanyId", "dbo.CompanyProfile");
            DropForeignKey("dbo.BidPackageXInvitee", "BidPackageId", "dbo.BidPackage");
            DropColumn("dbo.BidPackage", "CreatorId");
            DropColumn("dbo.Project", "ArchitectId");
            DropColumn("dbo.Project", "CreatedById");
            DropTable("dbo.BidPackageXInvitee");
        }
    }
}
