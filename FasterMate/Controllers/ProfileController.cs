namespace FasterMate.Controllers
{
    using FasterMate.Core.Constants;
    using FasterMate.Core.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
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
                return this.NotFound();
            }

            //TODO: Claims!

            return View(viewModel);
        }

    }
}
