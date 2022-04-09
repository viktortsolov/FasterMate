namespace FasterMate.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PostController : BaseController
    {

        //TODO: Manage posts
        public IActionResult ManagePosts()
        {
            return View();
        }
    }
}
