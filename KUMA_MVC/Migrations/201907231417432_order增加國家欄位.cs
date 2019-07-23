namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order增加國家欄位 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RecipientCountry", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "RecipientCountry");
        }
    }
}
