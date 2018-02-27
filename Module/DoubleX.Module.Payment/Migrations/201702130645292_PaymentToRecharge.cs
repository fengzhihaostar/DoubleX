namespace DoubleX.Module.Trade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentToRecharge : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "TB_PaymentRecord", newName: "TB_RechargeRecord");
        }
        
        public override void Down()
        {
            RenameTable(name: "TB_RechargeRecord", newName: "TB_PaymentRecord");
        }
    }
}
