
using EntityFrameworkTransactionScope.Data;
using EntityFrameworkTransactionScope.Data.Entity;
using System;
using System.Transactions;

namespace EntityFrameworkTransactionScope.DataAceess
{
    public class MakeReservation
    {

        FlightDBContext flight;

        HotelDBContext hotel;

        public MakeReservation()
        {
            flight = new FlightDBContext();
            hotel = new HotelDBContext();
        }

        //处理事务方法
        public bool ReservTrip(TripReservation trip)
        {
            bool reserved = false;

            //绑定处理事务范围
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    //航班信息
                    flight.FlightBookings.Add(trip.Filght);
                    flight.SaveChanges();

                    //预约信息
                    hotel.Reservations.Add(trip.Hotel);
                    hotel.SaveChanges();

                    reserved = true;

                    //完成事务并提交
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return reserved;
        }
    }
}