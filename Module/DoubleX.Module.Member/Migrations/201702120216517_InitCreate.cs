namespace DoubleX.Module.Member.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        RealName = c.String(maxLength: 200, storeType: "nvarchar"),
                        Sex = c.Int(nullable: false),
                        Email = c.String(maxLength: 200, storeType: "nvarchar"),
                        EmailIsVerify = c.Boolean(nullable: false),
                        Mobile = c.String(maxLength: 200, storeType: "nvarchar"),
                        MobileIsVerify = c.Boolean(nullable: false),
                        Credits = c.String(maxLength: 200, storeType: "nvarchar"),
                        Birthday = c.DateTime(nullable: false, precision: 0),
                        Country = c.String(maxLength: 200, storeType: "nvarchar"),
                        Area = c.String(maxLength: 200, storeType: "nvarchar"),
                        Address = c.String(maxLength: 200, storeType: "nvarchar"),
                        LastLoginIP = c.String(maxLength: 200, storeType: "nvarchar"),
                        LastLoginDt = c.DateTime(nullable: false, precision: 0),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        State = c.Int(nullable: false),
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
            DropTable("dbo.TB_Account");
        }
    }
}
