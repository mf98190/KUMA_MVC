namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_AddLineImgUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LineImgUrl", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LineImgUrl");
        }
    }
}
