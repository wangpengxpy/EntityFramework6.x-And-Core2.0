using System.Data.Entity.ModelConfiguration;
using EntityFrameworkTransactionScope.Data.Entity;

namespace EntityFrameworkTransactionScope.Data.Map
{
    public class FlightBookingMap : EntityTypeConfiguration<FlightBooking>
    {
        public FlightBookingMap()
        {
            //table
            ToTable("FlightBookings");

            //key
            HasKey(k => k.FlightId);

            //property
            Property(p => p.FilghtName).HasMaxLength(50);
            Property(p => p.Number);
            Property(p => p.TravellingDate);
        }
    }
}
