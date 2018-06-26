using EntityFrameworkTransactionScope.Data.Entity;
using EntityFrameworkTransactionScope.Data.Map;
using System.Data.Entity;

namespace EntityFrameworkTransactionScope.Data
{
    [DbConfigurationType(typeof(HotelFlightConfiguration))]
    public class FlightDBContext : DbContext
    {
        public FlightDBContext() : base("name=flightConnection")
        { }

        public DbSet<FlightBooking> FlightBookings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FlightBookingMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
