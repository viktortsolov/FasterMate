namespace FasterMate.Controllers
{
    using System.Diagnostics;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels;
    using FasterMate.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : Controller
    {
        private readonly IPostService postService;

        private readonly IRepository<Profile> profileRepo;

        public HomeController(
            IPostService _postService,
            IRepository<Profile> _profileRepo)
        {
            postService = _postService;
            profileRepo = _profileRepo;
        }

        public IActionResult Index()
        {
            var model = postService.RenderTimelinePosts();

            return View(model);
        }


        [Authorize]
        public IActionResult Search(string text)
        {
            //TODO: Extract this method in service (profile service or sth.)
            var models = profileRepo
                .AllAsNoTracking()
                .Include(x => x.Image)
                .Include(x => x.Country)
                .Select(x => new SearchProfileViewModel()
                {
                    ProfileId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ImagePath = x.Image != null ? $"{x.Image.Id}.{x.Image.Extension}" : null,
                    Country = x.Country.Name
                })
                .ToList();

            if (text != null)
            {
                models = models.Where(x =>
                    x.FirstName.Contains(text) ||
                    x.LastName.Contains(text) ||
                    x.Country.Contains(text))
                    .ToList();
            }

            return View(models);
        }

        //[HttpPost]
        //public IActionResult Search(string text)
        //{
        //    var models = profileRepo
        //        .AllAsNoTracking()
        //        .Include(x => x.Image)
        //        .Include(x => x.Country)
        //        .Select(x => new SearchProfileViewModel()
        //        {
        //            ProfileId = x.Id,
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            ImagePath = x.Image != null ? $"${x.Image.Id}.{x.Image.Extension}" : null,
        //            Country = x.Country.Name
        //        })
        //        .ToList();

        //    models = models.Where(x =>
        //        x.FirstName.Contains(text) ||
        //        x.LastName.Contains(text) ||
        //        x.Country.Contains(text))
        //        .ToList();

        //    return View(models);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}