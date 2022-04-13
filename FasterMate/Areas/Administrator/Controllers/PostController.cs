namespace FasterMate.Areas.Administrator.Controllers
{
    using FasterMate.Core.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class PostController : BaseController
    {
        private readonly IPostService postService;

        public PostController(IPostService _postService)
        {
            postService = _postService;
        }

        public async Task<IActionResult> ManagePosts()
        {
            var posts = await postService.PostListAdministratorAsync();

            return View(posts);
        }

        public async Task<IActionResult> DeletePost(string id)
        {
            await postService.DeletePostAdministratorAsync(id);

            return RedirectToAction(nameof(ManagePosts));
        }
    }
}
