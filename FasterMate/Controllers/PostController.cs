namespace FasterMate.Controllers
{
    using System.Security.Claims;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Comment;
    using FasterMate.ViewModels.Post;

    using Microsoft.AspNetCore.Mvc;

    public class PostController : BaseController
    {
        private readonly IProfileService profileService;
        private readonly IPostService postService;
        private readonly ICommentService commentService;

        private readonly IWebHostEnvironment webHost;

        public PostController(
            IProfileService _profileService,
            IPostService _postService,
            ICommentService _commentService,
            IWebHostEnvironment _webHost)
        {
            profileService = _profileService;
            postService = _postService;
            commentService = _commentService;

            webHost = _webHost;
        }

        public IActionResult CreatePost()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = new CreatePostViewModel
            {
                ReturnId = profileId
            };

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

        public IActionResult SeePost(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);
            var post = postService.RenderSinglePost(id, profileId);

            return View(post);
        }

        public async Task<IActionResult> LikePost(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await postService.LikePostAsync(profileId, id);

            return RedirectToAction("SeePost", "Post", new { id });
        }

        public IActionResult AddComment(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = new AddCommentViewModel
            {
                ReturnId = profileId,
                PostId = id
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await commentService.AddAsync(profileId, input);

            return RedirectToAction("SeePost", "Post", new { id = input.PostId });
        }

        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await postService.DeletePost(profileId, id);

            return RedirectToAction("UserProfile", "Profile", new { id = profileId });
        }

        public async Task<IActionResult> DeleteComment(string id)
        {
            var postId = await commentService.DeleteAsync(id);

            return RedirectToAction("SeePost", "Post", new { id = postId });
        }
    }
}
