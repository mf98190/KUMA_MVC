namespace KUMA_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLineUserID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LineUserID", c => c.String());
            DropTable("dbo.ViewWords");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ViewWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordName = c.String(nullable: false, maxLength: 128),
                        WordContent = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "LineUserID");
        }
    }
}
