namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 再試試4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Supports");
            AlterColumn("dbo.Supports", "SupportID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Supports", "SupportID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Supports");
            AlterColumn("dbo.Supports", "SupportID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Supports", "SupportID");
        }
    }
}
