namespace FasterMate.Controllers
{
    using FasterMate.Core.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;


        public UserController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            return Ok();
        }


        [Authorize(Roles = UserConstant.Administrator)]
        public async Task<IActionResult> ManageUsers()
        {

        }
    }
}
