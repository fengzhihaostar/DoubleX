namespace DoubleX.Module.Trade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentColumnToRecharge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_PaymentRecord", "RechargeState", c => c.Int(nullable: false));
            DropColumn("dbo.TB_PaymentRecord", "PaymentState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TB_PaymentRecord", "PaymentState", c => c.Int(nullable: false));
            DropColumn("dbo.TB_PaymentRecord", "RechargeState");
        }
    }
}
