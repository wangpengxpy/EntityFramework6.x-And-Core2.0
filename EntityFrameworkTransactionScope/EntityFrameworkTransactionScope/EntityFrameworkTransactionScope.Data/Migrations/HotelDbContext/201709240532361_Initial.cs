namespace EntityFrameworkTransactionScope.Data.Migrations.HotelDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        BookingId = c.Int(nullable: false),
                        Name = c.String(maxLength: 20),
                        BookingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reservations");
        }
    }
}
