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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var groupId = await groupService.CreateAsync(profileId, input, $"{webHost.WebRootPath}\\img\\groups");

            return RedirectToAction(nameof(LetsChat), new { id = groupId });
        }

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

        public IActionResult Members(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = new GroupMemberViewModel()
            {
                Id = id,
                Members = groupService.GetMembers(id, profileId)
            };


            return View(viewModel);
        }

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

        [HttpPost]
        public async Task<IActionResult> RemoveMember(string id, string removeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (isOwner)
            {
                await groupService.RemoveAsync(id, removeId);
            }

            return RedirectToAction(nameof(MyGroups), new { id });
        }

        public IActionResult AddMember(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (isOwner)
            {
                var viewModel = new FollowersToInviteViewModel()
                {
                    Id = id,
                    Followers = groupService.GetFollowersOfProfile(profileId, id)
                };

                return View(viewModel);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(string id, string addingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var isOwner = groupService.IsOwnerOfTheGroup(id, profileId);

            if (isOwner)
            {
                await groupService.AddMemberAsync(addingId, id);

                return RedirectToAction(nameof(AddMember), new { id });
            }

            return BadRequest();
        }
    }
}