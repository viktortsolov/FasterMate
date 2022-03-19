using Microsoft.AspNetCore.Mvc;

namespace FasterMate.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult UserProfile(int id)
        {
            return View();
        }
    }
}
