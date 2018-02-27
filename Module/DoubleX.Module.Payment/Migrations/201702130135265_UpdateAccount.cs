namespace DoubleX.Module.Trade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_CostRecord", "AccountId", c => c.Guid(nullable: false));
            AddColumn("dbo.TB_PaymentRecord", "AccountId", c => c.Guid(nullable: false));
            DropColumn("dbo.TB_CostRecord", "AccoutId");
            DropColumn("dbo.TB_PaymentRecord", "AccoutId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TB_PaymentRecord", "AccoutId", c => c.Guid(nullable: false));
            AddColumn("dbo.TB_CostRecord", "AccoutId", c => c.String(unicode: false));
            DropColumn("dbo.TB_PaymentRecord", "AccountId");
            DropColumn("dbo.TB_CostRecord", "AccountId");
        }
    }
}
