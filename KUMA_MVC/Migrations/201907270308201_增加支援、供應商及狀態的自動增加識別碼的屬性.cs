namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加支援供應商及狀態的自動增加識別碼的屬性 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Purchase", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseDetails", "PurchaseID", "dbo.Purchase");
            DropForeignKey("dbo.Orders", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Supports", "StatusID", "dbo.Status");
            DropPrimaryKey("dbo.Suppliers");
            DropPrimaryKey("dbo.Purchase");
            DropPrimaryKey("dbo.Status");
            DropPrimaryKey("dbo.Supports");
            AlterColumn("dbo.Suppliers", "SupplierID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Purchase", "PurchaseID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Status", "StasusID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Supports", "SupportID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Suppliers", "SupplierID");
            AddPrimaryKey("dbo.Purchase", "PurchaseID");
            AddPrimaryKey("dbo.Status", "StasusID");
            AddPrimaryKey("dbo.Supports", "SupportID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.Purchase", "SupplierID", "dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.PurchaseDetails", "PurchaseID", "dbo.Purchase", "PurchaseID");
            AddForeignKey("dbo.Orders", "StatusID", "dbo.Status", "StasusID");
            AddForeignKey("dbo.Supports", "StatusID", "dbo.Status", "StasusID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Supports", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Orders", "StatusID", "dbo.Status");
            DropForeignKey("dbo.PurchaseDetails", "PurchaseID", "dbo.Purchase");
            DropForeignKey("dbo.Purchase", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropPrimaryKey("dbo.Supports");
            DropPrimaryKey("dbo.Status");
            DropPrimaryKey("dbo.Purchase");
            DropPrimaryKey("dbo.Suppliers");
            AlterColumn("dbo.Supports", "SupportID", c => c.Int(nullable: false));
            AlterColumn("dbo.Status", "StasusID", c => c.Int(nullable: false));
            AlterColumn("dbo.Purchase", "PurchaseID", c => c.Int(nullable: false));
            AlterColumn("dbo.Suppliers", "SupplierID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Supports", "SupportID");
            AddPrimaryKey("dbo.Status", "StasusID");
            AddPrimaryKey("dbo.Purchase", "PurchaseID");
            AddPrimaryKey("dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.Supports", "StatusID", "dbo.Status", "StasusID");
            AddForeignKey("dbo.Orders", "StatusID", "dbo.Status", "StasusID");
            AddForeignKey("dbo.PurchaseDetails", "PurchaseID", "dbo.Purchase", "PurchaseID");
            AddForeignKey("dbo.Purchase", "SupplierID", "dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID");
        }
    }
}
