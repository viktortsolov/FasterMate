using FasterMate.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FasterMate.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;

        public ProfileController(
            IProfileService _profileService)
        {
            profileService = _profileService;
        }

        public IActionResult UserProfile(Guid id)
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
