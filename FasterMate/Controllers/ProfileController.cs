namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Profile;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class ProfileController : BaseController
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = profileService.RenderProfile(id, profileId);

            if (viewModel == null)
            {
                return NotFound();
            }

            viewModel.IsOwner = profileId == id;

            return View(viewModel);
        }

        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = profileService.GetByUserId(userId);
            var editViewModel = profileService.GetEditViewModel(user);

            return View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await profileService.UpdateAsync(userId, input, $"{webHost.WebRootPath}\\img\\users");
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
                return View(input);
            }

            return RedirectToAction(nameof(UserProfile), new { input.Id });
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
