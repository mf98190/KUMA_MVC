namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shippers加入Fare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Fare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Fare");
        }
    }
}
