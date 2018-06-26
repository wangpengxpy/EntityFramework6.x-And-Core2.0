namespace EntityFrameworkBaseExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Log : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        ErrorId = c.Int(nullable: false, identity: true),
                        Query = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Parameters = c.String(),
                        CommandType = c.String(maxLength: 8000, unicode: false),
                        TotalSeconds = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Exception = c.String(maxLength: 8000, unicode: false),
                        InnerException = c.String(maxLength: 8000, unicode: false),
                        RequestId = c.Int(nullable: false),
                        FileName = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ErrorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Errors");
        }
    }
}
