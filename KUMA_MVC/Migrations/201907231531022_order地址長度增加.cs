namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order地址長度增加 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "RecipientAddressee", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "RecipientAddressee", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
