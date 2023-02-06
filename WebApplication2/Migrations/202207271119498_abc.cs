namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplyForJobs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        message = c.String(),
                        dateTime = c.DateTime(nullable: false),
                        jobid = c.Int(nullable: false),
                        userid = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Jobs", t => t.jobid, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.userid)
                .Index(t => t.jobid)
                .Index(t => t.userid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplyForJobs", "userid", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplyForJobs", "jobid", "dbo.Jobs");
            DropIndex("dbo.ApplyForJobs", new[] { "userid" });
            DropIndex("dbo.ApplyForJobs", new[] { "jobid" });
            DropTable("dbo.ApplyForJobs");
        }
    }
}
