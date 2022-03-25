namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Profile;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;
        private readonly ICountryService countryService;

        private readonly IWebHostEnvironment webHost;

        public ProfileController(
            IProfileService _profileService,
            ICountryService _countryService, 
            IWebHostEnvironment _webHost)
        {
            profileService = _profileService;
            countryService = _countryService;

            webHost = _webHost;
        }

        public IActionResult UserProfile(string id)
        {
            var viewModel = profileService.RenderProfile(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = this.profileService.GetId(userId);

            viewModel.IsOwner = profileId == id;

            return View(viewModel);
        }

        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = profileService.GetByUserId(userId);
            var editViewModel = profileService.GetEditViewModel(user);

            return this.View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await this.profileService.UpdateAsync(userId, input, $"{this.webHost.WebRootPath}\\img\\users");
            }
            catch (ArgumentException ae)
            {
                this.ModelState.AddModelError(string.Empty, ae.Message);
                return this.View(input);
            }

            return this.RedirectToAction(nameof(UserProfile), new { input.Id });
        }

        public IActionResult Search()
        {
            return View(null);
        }

        public async Task<IActionResult> Follow(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await profileService.FollowProfileAsync(id, profileId);

            return RedirectToAction(nameof(UserProfile), new { id });
        }
    }
}
