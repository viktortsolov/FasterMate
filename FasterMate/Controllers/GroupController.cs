namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Group;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class GroupController : BaseController
    {
        private readonly IGroupService groupService;
        private readonly IProfileService profileService;

        private readonly IWebHostEnvironment webHost;

        public GroupController(
            IGroupService _groupService,
            IProfileService _profileService,
            IWebHostEnvironment _webHost)
        {
            groupService = _groupService;
            profileService = _profileService;

            webHost = _webHost;
        }

        public IActionResult LetsChat(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isMember = groupService.IsMemberOfTheGroup(id, profileId);

            if (isMember)
            {
                var viewModel = groupService.GetById(id);
                viewModel.ProfileId = profileId;

                return View(viewModel);
            }

            return BadRequest();
        }

        public IActionResult Create()
        {
            var viewModel = new CreateGroupViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userid);

            var groupId = await groupService.CreateAsync(profileId, input, $"{webHost.WebRootPath}/img/groups");

            return RedirectToAction(nameof(LetsChat), new { id = groupId });
        }
    }
}
