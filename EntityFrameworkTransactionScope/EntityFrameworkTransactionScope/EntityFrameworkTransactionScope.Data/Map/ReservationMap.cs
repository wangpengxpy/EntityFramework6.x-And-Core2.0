using System.Data.Entity.ModelConfiguration;
using EntityFrameworkTransactionScope.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkTransactionScope.Data.Map
{
    public class ReservationMap : EntityTypeConfiguration<Reservation>
    {
        public ReservationMap()
        {
            //table
            ToTable("Reservations");

            //key
            HasKey(k => k.BookingId);

            //property
            Property(p => p.BookingId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Name).HasMaxLength(20);
            Property(p => p.BookingDate);
        }
    }
}
