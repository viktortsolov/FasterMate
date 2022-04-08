namespace FasterMate.Controllers
{
    using System.Diagnostics;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels;
    using FasterMate.ViewModels.Home;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IPostService postService;
        private readonly IProfileService profileService;

        public HomeController(
            IPostService _postService,
            IProfileService _profileService)
        {
            postService = _postService;
            profileService = _profileService;
        }

        public IActionResult Index()
        {
            var model = postService.RenderTimelinePosts();

            return View(model);
        }


        [Authorize]
        public IActionResult Search(string searchText)
        {
            var viewModel = new SearchViewModel { Profiles = new List<ProfileSearchViewModel>() };

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchTokens = searchText.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                viewModel.Profiles = profileService.SearchProfiles(searchTokens);
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}