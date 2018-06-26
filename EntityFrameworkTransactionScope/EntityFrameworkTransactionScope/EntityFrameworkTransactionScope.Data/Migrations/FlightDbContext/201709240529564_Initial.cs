namespace EntityFrameworkTransactionScope.Data.Migrations.FlightDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlightBookings",
                c => new
                    {
                        FlightId = c.Int(nullable: false, identity: true),
                        FilghtName = c.String(maxLength: 50),
                        Number = c.String(),
                        TravellingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FlightId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FlightBookings");
        }
    }
}
