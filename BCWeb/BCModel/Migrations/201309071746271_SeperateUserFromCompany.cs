namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeperateUserFromCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyProfile",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(nullable: false),
                    Address1 = c.String(),
                    Address2 = c.String(),
                    City = c.String(),
                    PostalCode = c.String(nullable: false),
                    StateId = c.Int(),
                    CountyId = c.Int(),
                    Phone = c.String(nullable: false),
                    ContactEmail = c.String(),
                    Published = c.Boolean(nullable: false),
                    OperatingDistance = c.Int(nullable: false),
                    BusinessTypeId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId)
                .ForeignKey("dbo.County", t => t.CountyId)
                .ForeignKey("dbo.BusinessType", t => t.BusinessTypeId)
                .Index(t => t.StateId)
                .Index(t => t.CountyId)
                .Index(t => t.BusinessTypeId);





            AddColumn("dbo.UserProfile", "CompanyId", c => c.Int(nullable: false));

            // insert data before indexing or applying relationship since the column cannot be null
            Sql(@"declare @CompanyName as varchar(100), @Address1 as varchar(50),@Address2 as varchar(50),@City as varchar(50),@PostalCode as varchar(10),@Phone as varchar(15);" + System.Environment.NewLine +
                @"declare @StateId as int, @CountyId as int,@OperatingDistance as int,@BusinessTypeId as int, @UserId as int, @CompanyId as int;" + System.Environment.NewLine +
                @"declare @Published as bit, @exists as bit;" + System.Environment.NewLine +
                @"declare comp_cur Cursor for select UserId, CompanyName,Address1,Address2,City,PostalCode,StateId,CountyId,Phone,Published,OperatingDistance,BusinessTypeId from UserProfile ;" + System.Environment.NewLine +
                @"open comp_cur;" + System.Environment.NewLine +
                @"fetch next from comp_cur into @UserId,@CompanyName,@Address1,@Address2,@City,@PostalCode,@StateId,@CountyId,@Phone,@Published,@OperatingDistance,@BusinessTypeId;" + System.Environment.NewLine +
                @"while @@FETCH_STATUS = 0" + System.Environment.NewLine +
                @"begin" + System.Environment.NewLine +

                    @"select @exists = 1 from CompanyProfile where CompanyName = @CompanyName and Phone = @Phone and BusinessTypeId = @BusinessTypeId" + System.Environment.NewLine +
                    
                // FIXME: NEED TO BE CONDITIONAL ABOUT IN SERTS.  SHOULD INSERT FROM MANAGER RECORD.  NEED TO UPDATE EMPLOYEE RECORDS WITH OUT INSERTING ANOTHER BUSINESS
                    @"if (@exists is not null and @exists = 1)" + System.Environment.NewLine +
                    @"begin" + System.Environment.NewLine +
                        @"print 'exists'" + System.Environment.NewLine +
                        @"select @CompanyId = Id from CompanyProfile where CompanyName = @CompanyName and Phone = @Phone and BusinessTypeId = @BusinessTypeId" + System.Environment.NewLine +
                    @"end" + System.Environment.NewLine +
                    @"else" + System.Environment.NewLine +
                    @"begin" + System.Environment.NewLine +
                        @"print 'not exists'" + System.Environment.NewLine +
                        @"Insert dbo.CompanyProfile(CompanyName,Address1,Address2,City,PostalCode,StateId,CountyId,Phone,Published,OperatingDistance,BusinessTypeId)" + System.Environment.NewLine +
                        @"values(@CompanyName,@Address1,@Address2,@City,@PostalCode,@StateId,@CountyId,@Phone,@Published,@OperatingDistance,@BusinessTypeId);" + System.Environment.NewLine +
                        @"set @CompanyId = @@Identity;" + System.Environment.NewLine +
                    @"end" + System.Environment.NewLine +

                    @"update UserProfile" + System.Environment.NewLine +
                    @"set CompanyId = @CompanyId" + System.Environment.NewLine +
                    @"Where UserId = @UserId;" + System.Environment.NewLine +
                    
                @"fetch next from comp_cur into @UserId,@CompanyName,@Address1,@Address2,@City,@PostalCode,@StateId,@CountyId,@Phone,@Published,@OperatingDistance,@BusinessTypeId;" + System.Environment.NewLine +
                @"end" + System.Environment.NewLine +
                @"close comp_cur" + System.Environment.NewLine +
                @"deallocate comp_cur"
                );

            AddColumn("dbo.Scope", "CompanyProfile_Id", c => c.Int());
            AddForeignKey("dbo.UserProfile", "CompanyId", "dbo.CompanyProfile", "Id");
            AddForeignKey("dbo.Scope", "CompanyProfile_Id", "dbo.CompanyProfile", "Id");
            CreateIndex("dbo.UserProfile", "CompanyId");
            CreateIndex("dbo.Scope", "CompanyProfile_Id");





            DropForeignKey("dbo.UserProfile", "BusinessTypeId", "dbo.BusinessType");
            DropForeignKey("dbo.UserProfile", "ManagerId", "dbo.UserProfile");
            DropIndex("dbo.UserProfile", new[] { "BusinessTypeId" });
            DropIndex("dbo.UserProfile", new[] { "ManagerId" });
            RenameColumn(table: "dbo.UserProfile", name: "CountyId", newName: "County_Id");
            RenameColumn(table: "dbo.UserProfile", name: "StateId", newName: "State_Id");



            DropColumn("dbo.UserProfile", "CompanyName");
            DropColumn("dbo.UserProfile", "Address1");
            DropColumn("dbo.UserProfile", "Address2");
            DropColumn("dbo.UserProfile", "City");
            DropColumn("dbo.UserProfile", "PostalCode");
            DropColumn("dbo.UserProfile", "Phone");
            DropColumn("dbo.UserProfile", "Published");
            DropColumn("dbo.UserProfile", "OperatingDistance");
            DropColumn("dbo.UserProfile", "BusinessTypeId");
            DropColumn("dbo.UserProfile", "ManagerId");
        }

        public override void Down()
        {
            AddColumn("dbo.UserProfile", "ManagerId", c => c.Int());
            AddColumn("dbo.UserProfile", "BusinessTypeId", c => c.Int());
            AddColumn("dbo.UserProfile", "OperatingDistance", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "Published", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserProfile", "Phone", c => c.String(nullable: false));
            AddColumn("dbo.UserProfile", "PostalCode", c => c.String(nullable: false));
            AddColumn("dbo.UserProfile", "City", c => c.String());
            AddColumn("dbo.UserProfile", "Address2", c => c.String());
            AddColumn("dbo.UserProfile", "Address1", c => c.String());
            AddColumn("dbo.UserProfile", "CompanyName", c => c.String(nullable: false));
            Sql(@"update up" + System.Environment.NewLine +
                @"set up.BusinessTypeId = cp.BusinessTypeId," + System.Environment.NewLine +
                @"up.OperatingDistance = cp.OperatingDistance," + System.Environment.NewLine +
                @"up.Published = cp.Published," + System.Environment.NewLine +
                @"up.Phone = cp.Phone," + System.Environment.NewLine +
                @"up.PostalCode = cp.PostalCode," + System.Environment.NewLine +
                @"up.City = cp.City," + System.Environment.NewLine +
                @"up.Address1 = cp.Address1," + System.Environment.NewLine +
                @"up.Address2 = cp.Address2," + System.Environment.NewLine +
                @"up.CompanyName = cp.CompanyName," + System.Environment.NewLine +
                @"up.ManagerId = (" + System.Environment.NewLine +
                    @"select up2.UserId from UserProfile up2 " + System.Environment.NewLine +
                    @"join CompanyProfile cp2" + System.Environment.NewLine +
                    @"on cp2.Id = up2.CompanyId" + System.Environment.NewLine +
                    @"join webpages_UsersInRoles ur2" + System.Environment.NewLine +
                    @"on up2.UserId = ur2.UserId" + System.Environment.NewLine +
                    @"join webpages_Roles r2" + System.Environment.NewLine +
                    @"on ur2.RoleId = r2.RoleId" + System.Environment.NewLine +
                    @"where r2.RoleName in ('Manager')" + System.Environment.NewLine +
                    @"and cp2.Id = cp.id" + System.Environment.NewLine +
                    @"and up2.UserId != up.UserId)" + System.Environment.NewLine +
                @"from dbo.UserProfile up" + System.Environment.NewLine +
                @"join CompanyProfile cp" + System.Environment.NewLine +
                @"on cp.Id = up.CompanyId" + System.Environment.NewLine);


            DropIndex("dbo.Scope", new[] { "CompanyProfile_Id" });
            DropIndex("dbo.CompanyProfile", new[] { "BusinessTypeId" });
            DropIndex("dbo.CompanyProfile", new[] { "CountyId" });
            DropIndex("dbo.CompanyProfile", new[] { "StateId" });
            DropIndex("dbo.UserProfile", new[] { "CompanyId" });
            DropForeignKey("dbo.Scope", "CompanyProfile_Id", "dbo.CompanyProfile");
            DropForeignKey("dbo.CompanyProfile", "BusinessTypeId", "dbo.BusinessType");
            DropForeignKey("dbo.CompanyProfile", "CountyId", "dbo.County");
            DropForeignKey("dbo.CompanyProfile", "StateId", "dbo.State");
            DropForeignKey("dbo.UserProfile", "CompanyId", "dbo.CompanyProfile");
            DropColumn("dbo.Scope", "CompanyProfile_Id");
            DropColumn("dbo.UserProfile", "CompanyId");
            DropTable("dbo.CompanyProfile");
            RenameColumn(table: "dbo.UserProfile", name: "State_Id", newName: "StateId");
            RenameColumn(table: "dbo.UserProfile", name: "County_Id", newName: "CountyId");
            CreateIndex("dbo.UserProfile", "ManagerId");
            CreateIndex("dbo.UserProfile", "BusinessTypeId");
            AddForeignKey("dbo.UserProfile", "ManagerId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.UserProfile", "BusinessTypeId", "dbo.BusinessType", "Id");
        }
    }
}
