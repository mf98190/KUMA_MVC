namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order增加運費欄位1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Fare_now", c => c.Decimal(nullable: false, storeType: "money"));
            DropColumn("dbo.Orders", "Fare");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Fare", c => c.Decimal(nullable: false, storeType: "money"));
            DropColumn("dbo.Orders", "Fare_now");
        }
    }
}
