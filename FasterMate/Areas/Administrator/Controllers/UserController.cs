namespace FasterMate.Areas.Administrator.Controllers
{
    using System.Web.Mvc;

    using FasterMate.Core.Constants;
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.User;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserService userService;


        public UserController(
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IUserService _userService
            )
        {
            roleManager = _roleManager;
            userManager = _userManager;

            userService = _userService;
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    //await roleManager.CreateAsync(new IdentityRole()
        //    //{
        //    //    Name = "Administrator"
        //    //});

        //    return Ok();
        //}

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

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
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

        //TODO: Finish the roles
        public async Task<IActionResult> Roles(string id)
        {
            var user = await userManager.GetUserAsync(User);

            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                });

            return Ok();
        }
    }
}
