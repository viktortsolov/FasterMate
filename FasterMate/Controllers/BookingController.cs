namespace FasterMate.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BookingController : BaseController
    {
        public IActionResult BookAFlight()
        {
            return View();
        }

        public IActionResult MyFlights()
        {
            return View();
        }
    }
}
