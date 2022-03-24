namespace FasterMate.Areas.Administrator.Controllers
{
    using FasterMate.Core.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize(Roles = UserConstant.Roles.Administrator)]
    [Area("Administrator")]
    public class BaseController : Controller
    {

    }
}
