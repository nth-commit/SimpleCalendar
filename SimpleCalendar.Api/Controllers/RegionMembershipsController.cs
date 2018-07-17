using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Commands.RegionMemberships;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Authorize]
    [Route("regionmemberships")]
    public class RegionMembershipsController : Controller
    {
        private readonly Lazy<IQueryRegionMembershipCommand> _queryRegionMembershipCommand;
        private readonly Lazy<ICreateRegionMembershipCommand> _createRegionMembershipCommand;
        private readonly Lazy<IDeleteRegionMembershipCommand> _deleteRegionMembershipCommand;

        public RegionMembershipsController(
            Lazy<IQueryRegionMembershipCommand> queryRegionMembershipCommand,
            Lazy<ICreateRegionMembershipCommand> createRegionMembershipCommand,
            Lazy<IDeleteRegionMembershipCommand> deleteRegionMembershipCommand)
        {
            _queryRegionMembershipCommand = queryRegionMembershipCommand;
            _createRegionMembershipCommand = createRegionMembershipCommand;
            _deleteRegionMembershipCommand = deleteRegionMembershipCommand;
        }

        [HttpGet("")]
        public Task<IActionResult> Query([FromQuery] string regionId, [FromQuery] string userEmail) =>
            _queryRegionMembershipCommand.Value.InvokeAsync(ControllerContext, regionId, userEmail);

        [HttpGet("my")]
        public Task<IActionResult> QueryMy([FromQuery] string regionId) =>
            _queryRegionMembershipCommand.Value.InvokeAsync(ControllerContext, regionId, User.GetUserEmail());

        [HttpPost("")]
        public Task<IActionResult> Create([FromBody] RegionMembershipCreate create) =>
            _createRegionMembershipCommand.Value.InvokeAsync(ControllerContext, create);

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete([FromRoute] string id) =>
            _deleteRegionMembershipCommand.Value.InvokeAsync(ControllerContext, id);
    }
}
