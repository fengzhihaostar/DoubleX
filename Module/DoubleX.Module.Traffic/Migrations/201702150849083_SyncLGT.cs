namespace DoubleX.Module.Traffic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncLGT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_Traffic", "Src", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_Traffic", "Src");
        }
    }
}
