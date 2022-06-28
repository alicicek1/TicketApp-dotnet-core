using Microsoft.AspNetCore.Mvc;
using TicketApp.Application.Interfaces;
using TicketApp.Core.Util.Filter;

namespace TicketApp.Web.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DataFilter dataFilter)
        {
            return Ok(_userService.GetAll(dataFilter));
        }
    }
}