namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 修正加入的位置加入Fare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shippers", "Fare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Suppliers", "Fare");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suppliers", "Fare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Shippers", "Fare");
        }
    }
}
