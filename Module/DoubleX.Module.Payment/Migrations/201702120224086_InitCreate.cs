namespace DoubleX.Module.Trade.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_CostRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccoutId = c.String(unicode: false),
                        CostType = c.Int(nullable: false),
                        CostTypeRefValue = c.String(maxLength: 200, storeType: "nvarchar"),
                        MoneyValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descript = c.String(maxLength: 600, storeType: "nvarchar"),
                        IsDelete = c.Boolean(nullable: false),
                        CreateId = c.Guid(nullable: false),
                        CreateDt = c.DateTime(nullable: false, precision: 0),
                        LastId = c.Guid(nullable: false),
                        LastDt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TB_PaymentRecord",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccoutId = c.Guid(nullable: false),
                        PaymentType = c.Int(nullable: false),
                        PaymentTypeRefValue = c.String(maxLength: 200, storeType: "nvarchar"),
                        MoneyValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descript = c.String(maxLength: 600, storeType: "nvarchar"),
                        PaymentState = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        CreateId = c.Guid(nullable: false),
                        CreateDt = c.DateTime(nullable: false, precision: 0),
                        LastId = c.Guid(nullable: false),
                        LastDt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TB_PaymentRecord");
            DropTable("dbo.TB_CostRecord");
        }
    }
}
