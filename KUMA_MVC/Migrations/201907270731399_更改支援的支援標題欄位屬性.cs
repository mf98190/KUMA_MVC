namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 更改支援的支援標題欄位屬性 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Supports", "SupportTitle", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Supports", "SupportTitle", c => c.String(nullable: false, maxLength: 128, fixedLength: true));
        }
    }
}
