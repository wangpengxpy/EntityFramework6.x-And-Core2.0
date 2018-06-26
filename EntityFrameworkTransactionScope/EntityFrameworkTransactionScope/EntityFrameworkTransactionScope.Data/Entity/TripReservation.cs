namespace EntityFrameworkTransactionScope.Data.Entity
{
    public class TripReservation
    {
        public FlightBooking Filght { get; set; }
        public Reservation Hotel { get; set; }
    }
}
