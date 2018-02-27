namespace DoubleX.Module.Traffic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccountId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_Traffic", "AccountId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_Traffic", "AccountId");
        }
    }
}
