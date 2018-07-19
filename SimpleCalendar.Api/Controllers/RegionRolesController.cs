using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Commands.RegionRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Authorize]
    [Route("regionroles")]
    public class RegionRolesController : Controller
    {
        private readonly Lazy<IQueryRegionRoleCommand> _regionRoleQueryCommand;

        public RegionRolesController(
            Lazy<IQueryRegionRoleCommand> regionRoleQueryCommand)
        {
            _regionRoleQueryCommand = regionRoleQueryCommand;
        }

        [HttpGet("")]
        public Task<IActionResult> Query() =>
            _regionRoleQueryCommand.Value.InvokeAsync(ControllerContext);
    }
}
