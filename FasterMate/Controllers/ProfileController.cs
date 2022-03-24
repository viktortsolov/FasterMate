namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(
            IProfileService _profileService)
        {
            profileService = _profileService;
        }

        public IActionResult UserProfile(string id)
        {
            var viewModel = profileService.RenderProfile(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            //TODO: Claims!

            return View(viewModel);
        }

    }
}
