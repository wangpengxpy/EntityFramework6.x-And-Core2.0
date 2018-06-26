using EntityFrameworkTransactionScope.Data.Entity;
using EntityFrameworkTransactionScope.Data.Map;
using System.Data.Entity;

namespace EntityFrameworkTransactionScope.Data
{
    [DbConfigurationType(typeof(HotelFlightConfiguration))]
    public class HotelDBContext: DbContext
    {
        public HotelDBContext():base("name=reservationConnction")
        { }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ReservationMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
