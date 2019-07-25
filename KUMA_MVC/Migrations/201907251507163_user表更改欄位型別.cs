namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user表更改欄位型別 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 15));
            AlterColumn("dbo.AspNetUsers", "Country", c => c.String(maxLength: 15));
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "BankAccount", c => c.String(maxLength: 20));
            AlterColumn("dbo.AspNetUsers", "CreditCard", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CreditCard", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.AspNetUsers", "BankAccount", c => c.String(maxLength: 20, fixedLength: true));
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 10, fixedLength: true));
            AlterColumn("dbo.AspNetUsers", "Country", c => c.String(maxLength: 15, fixedLength: true));
            AlterColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 15, fixedLength: true));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 30, fixedLength: true));
        }
    }
}
