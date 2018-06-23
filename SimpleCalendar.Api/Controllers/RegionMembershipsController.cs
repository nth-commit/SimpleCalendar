using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions.Authorization;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("regionmemberships")]
    public class RegionMembershipsController : Controller
    {
        protected readonly IAuthorizationService _authorizationService;
        protected readonly CoreDbContext _coreDbContext;

        public RegionMembershipsController(
            IAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        public async Task<IActionResult> Create([FromBody] RegionMembershipCreate create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var region = await _coreDbContext.GetRegionByCodesAsync(create.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(RegionMembershipCreate.RegionId), "Region not found");
                return BadRequest(ModelState);
            }

            var canCreateMembership = (await _authorizationService.AuthorizeAsync(User, region, new CreateMembershipRequirement())).Succeeded;
            if (!canCreateMembership)
            {
                return Unauthorized();
            }

            await Task.CompletedTask;
            return Created("", new { });
        }
    }
}
