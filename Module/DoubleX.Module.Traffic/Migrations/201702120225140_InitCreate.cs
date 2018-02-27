namespace DoubleX.Module.Traffic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Traffic",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(unicode: false),
                        TranslateKey = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Size = c.Long(nullable: false),
                        PlatformCode = c.Int(nullable: false),
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
            DropTable("dbo.TB_Traffic");
        }
    }
}
