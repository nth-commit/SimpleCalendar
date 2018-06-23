using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions.Authorization;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework.Identity;
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

            var canCreateMembership = (await _authorizationService.AuthorizeAsync(User, region, RegionRequirement.CreateMembership(create.Role))).Succeeded;
            if (!canCreateMembership)
            {
                return Unauthorized();
            }

            var roleExists = await _coreDbContext.RegionRoles
                .Where(r => r.RegionId == region.Id)
                .Where(r => r.UserId == create.UserId)
                .AnyAsync();

            if (roleExists)
            {
                ModelState.AddModelError(
                    $"{nameof(RegionMembershipCreate.RegionId)}+{nameof(RegionMembershipCreate.UserId)}",
                    "Region not found");
                return BadRequest(ModelState);
            }

            var result = await _coreDbContext.RegionRoles.AddAsync(new RegionRoleEntity()
            {
                UserId = create.UserId,
                RegionId = region.Id,
                Role = (Role)create.Role
            });

            return Created(
                "",
                new RegionMembership()
                {
                    Id = result.Entity.Id,
                    UserId = result.Entity.UserId,
                    RegionId = create.RegionId,
                    Role = create.Role
                });
        }
    }
}
