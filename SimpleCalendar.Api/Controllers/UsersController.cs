using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Commands.Users;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Authorize]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly Lazy<IGetUserCommand> _getUserCommand;

        public UsersController(
            Lazy<IGetUserCommand> getUserCommand)
        {
            _getUserCommand = getUserCommand;
        }

        [HttpGet("{email}")]
        public Task<IActionResult> Get([FromQuery] string email) =>
            _getUserCommand.Value.InvokeAsync(ControllerContext, email);

        [HttpGet("me")]
        public Task<IActionResult> GetMyself() =>
            _getUserCommand.Value.InvokeAsync(ControllerContext, User.GetUserEmail());
    }
}
