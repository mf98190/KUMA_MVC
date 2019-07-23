namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order增加運費欄位 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Fare", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Fare");
        }
    }
}
