namespace FasterMate.Controllers
{
    using System.Security.Claims;

    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
    }
}
