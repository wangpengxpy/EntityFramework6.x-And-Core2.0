using EntityFrameworkTransactionScope.Data.Entity;
using EntityFrameworkTransactionScope.DataAceess;
using System;
using System.Web.Mvc;

namespace EntityFrameworkTransactionScope.Controllers
{
    public class TripController : Controller
    {
        MakeReservation reserv;

        public TripController()
        {
            reserv = new MakeReservation();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new TripReservation());
        }

        [HttpPost]
        public ActionResult Create(TripReservation tripinfo)
        {
            try
            {
                tripinfo.Filght.TravellingDate = DateTime.Now;
                tripinfo.Hotel.BookingDate = DateTime.Now;
                var res = reserv.ReservTrip(tripinfo);

                if (!res)
                {
                    return View("Error");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return View("Success");
        }
    }
}