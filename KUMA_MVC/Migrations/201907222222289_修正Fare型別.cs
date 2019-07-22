namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 修正Fare型別 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shippers", "Fare", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shippers", "Fare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
