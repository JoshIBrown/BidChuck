namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserToCompanyInBid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bid", "UserId", "dbo.UserProfile");
            DropIndex("dbo.Bid", new[] { "UserId" });
            AddColumn("dbo.Bid", "CompanyId", c => c.Int(nullable: false));
            Sql(@"update b
                set b.CompanyId = u.CompanyId
                from dbo.Bid b
                join dbo.UserProfile u
                on b.UserId = u.UserId");
            AddForeignKey("dbo.Bid", "CompanyId", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.Bid", "CompanyId");
            DropColumn("dbo.Bid", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bid", "UserId", c => c.Int(nullable: false));
            Sql(@"update b
                  set b.UserId = u.UserId
                    from dbo.Bid b
                    join dbo.UserProfile u
                    on u.CompanyId = b.CompanyId
                    join webpages_UsersInRole ur
                    on ur.UserId = u.UserId
                    join webpages_Roles r
                    on ur.RoleId = r.RoleId
                    where r.RoleName='Manager'");
            DropIndex("dbo.Bid", new[] { "CompanyId" });
            DropForeignKey("dbo.Bid", "CompanyId", "dbo.CompanyProfile");
            DropColumn("dbo.Bid", "CompanyId");
            CreateIndex("dbo.Bid", "UserId");
            AddForeignKey("dbo.Bid", "UserId", "dbo.UserProfile", "UserId");
        }
    }
}
