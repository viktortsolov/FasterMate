namespace FasterMate.Areas.Administrator.Controllers
{
    using FasterMate.Core.Constants;
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.User;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IRepository<ApplicationUser> appUserRepo;

        private readonly IUserService userService;


        public UserController(
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService,
            IRepository<ApplicationUser> _appUserRepo
            )
        {
            roleManager = _roleManager;
            userManager = _userManager;

            appUserRepo = _appUserRepo;

            userService = _userService;
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsers();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetUserForEdit(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await userService.UpdateUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Saved changes.";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Error ocured!";
            }

            return View(model);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserById(id);
            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Name = $"{user.Profile.FirstName} {user.Profile.LastName}"
            };

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name,
                    Selected = userManager.IsInRoleAsync(user, x.Name).Result
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesViewModel model)
        {
            var user = await userService.GetOnlyUserById(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);

            //TODO: Exract In Method
            appUserRepo.Update(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userManager.AddToRolesAsync(user, model.RoleNames);

            return RedirectToAction(nameof(ManageUsers));
        }
    }
}
