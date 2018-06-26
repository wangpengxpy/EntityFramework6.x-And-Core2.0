namespace EntityFrameworkBaseExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ByteArray : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "BannerImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "BannerImage");
        }
    }
}
