namespace EntityFramework6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingDetails",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        Number = c.String(),
                        BankName = c.String(),
                        Swift = c.String(),
                        CardType = c.Int(),
                        ExpiryMonth = c.String(),
                        ExpiryYear = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.BillingDetailId);
            
            AlterColumn("dbo.Customers", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "ModifiedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "ModifiedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Students", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Students", "ModifiedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Courses", "ModifiedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Courses", "CreatedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Students", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Students", "CreatedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Orders", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Orders", "CreatedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Customers", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Customers", "CreatedTime", c => c.DateTime(nullable: false, storeType: "date"));
            DropTable("dbo.BillingDetails");
        }
    }
}
