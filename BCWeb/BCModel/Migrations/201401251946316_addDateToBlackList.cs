namespace BCModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateToBlackList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlackList", "BlackListDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlackList", "BlackListDate");
        }
    }
}
