namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Post;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class PostController : BaseController
    {
        private readonly IProfileService profileService;
        private readonly IPostService postService;

        private readonly IWebHostEnvironment webHost;

        public PostController(
            IProfileService _profileService,
            IPostService _postService,
            IWebHostEnvironment _webHost)
        {
            profileService = _profileService;
            postService = _postService;

            webHost = _webHost;
        }

        public IActionResult CreatePost()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = new CreatePostViewModel();
            viewModel.ReturnId = profileId;
            viewModel.IsProfile = true;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await postService.CreateAsync(profileId, input, $"{webHost.WebRootPath}\\img\\posts");

            return RedirectToAction("UserProfile", "Profile", new { id = profileId });
        }
    }
}
