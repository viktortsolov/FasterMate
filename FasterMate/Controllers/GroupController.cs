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
                var viewModel = groupService.GetGroupById(id);
                viewModel.ProfileId = profileId;

                return View(viewModel);
            }

            return BadRequest();
        }

        //DONE
        public IActionResult Create()
        {
            var viewModel = new CreateGroupViewModel();

            return View(viewModel);
        }
        //DONE
        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var groupId = await groupService.CreateAsync(profileId, input, $"{webHost.WebRootPath}\\img\\groups");

            return RedirectToAction(nameof(LetsChat), new { id = groupId });
        }
        //DONE
        public IActionResult MyGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = groupService.GetProfileGroups(profileId).ToList();
            foreach (var group in viewModel)
            {
                group.IsOwner = groupService.IsOwnerOfTheGroup(group.Id, profileId);
            }

            return View(viewModel);
        }
        //DONE
        public IActionResult Members(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = groupService.GetMembers(id, profileId);

            return View(viewModel);
        }
        //DONE
        public IActionResult Edit(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (isOwner)
            {
                var inputModel = groupService.GetGroupForEdit(id);

                return View(inputModel);
            }

            return BadRequest();
        }
        //DONE
        [HttpPost]
        public async Task<IActionResult> Edit(EditGroupViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(input.Id, profileId);

            if (isOwner)
            {
                await groupService.UpdateAsync(input, $"{webHost.WebRootPath}\\img\\groups");
            }

            return RedirectToAction(nameof(MyGroups));
        }
        //DONE
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (isOwner)
            {
                await groupService.DeleteGroupAsync(id);
            }

            return RedirectToAction(nameof(MyGroups));
        }

        public async Task<IActionResult> Leave(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (!isOwner)
            {
                await groupService.LeaveAsync(id, profileId);
            }

            return RedirectToAction(nameof(MyGroups));
        }
    }
}
