namespace FasterMate.Controllers
{
    using System.Diagnostics;
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IPostService postService;

        public HomeController(IPostService _postService)
        {
            postService = _postService;
        }

        public IActionResult Index()
        {
            var model = postService.RenderTimelinePosts();

            return View(model);
        }


        //TODO: Search
        [Authorize]
        public IActionResult Search(string text, string id)
        {
            //if (text != null && text != string.Empty)
            //{
            //    var searchTokens = text.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            //    var 

            //    var viewModel = new SearchProfileViewModel()
            //    {

            //    };

            //    return this.View(viewModel);
            //}

            return this.RedirectToAction(nameof(Search)/*, IEnumerable<SearchProfileViewModel>*/);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}