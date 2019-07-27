namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加支援供應商及狀態的自動增加識別碼的屬性y94u6h4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Supports", "SupportCategoryID", "dbo.SupportCategory");
            DropPrimaryKey("dbo.SupportCategory");
            AlterColumn("dbo.SupportCategory", "SupportCategoryID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SupportCategory", "SupportCategoryID");
            AddForeignKey("dbo.Supports", "SupportCategoryID", "dbo.SupportCategory", "SupportCategoryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Supports", "SupportCategoryID", "dbo.SupportCategory");
            DropPrimaryKey("dbo.SupportCategory");
            AlterColumn("dbo.SupportCategory", "SupportCategoryID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SupportCategory", "SupportCategoryID");
            AddForeignKey("dbo.Supports", "SupportCategoryID", "dbo.SupportCategory", "SupportCategoryID");
        }
    }
}
