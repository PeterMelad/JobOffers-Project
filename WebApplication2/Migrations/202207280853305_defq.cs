namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class defq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "publisherId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Jobs", "publisherId");
            AddForeignKey("dbo.Jobs", "publisherId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "publisherId", "dbo.AspNetUsers");
            DropIndex("dbo.Jobs", new[] { "publisherId" });
            DropColumn("dbo.Jobs", "publisherId");
        }
    }
}
