namespace DoubleX.Module.Organize.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Admin",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        LoginCount = c.Int(nullable: false),
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
            DropTable("dbo.TB_Admin");
        }
    }
}
