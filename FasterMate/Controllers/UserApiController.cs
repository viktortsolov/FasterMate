namespace FasterMate.Controllers
{
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Common;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService service;

        public UserApiController(IUserService _service)
        {
            service = _service;
        }

        [HttpGet]
        public ActionResult<ICollection<ApiViewModel>> Get()
            => service.GetAPIData();
    }
}
