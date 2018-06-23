using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions.Authorization;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework;
using SimpleCalendar.Framework.Identity;
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
        protected readonly IAuthorizationService _authorizationService;
        protected readonly CoreDbContext _coreDbContext;

        public RegionMembershipsController(
            IAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Query(
            [FromQuery] string regionId,
            [FromQuery] string userId)
        {
            if (!await _coreDbContext.IsAnyAdministratorAsync(User.GetUserId()))
            {
                return Unauthorized();
            }

            var query = _coreDbContext.RegionRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(regionId))
            {
                var region = await _coreDbContext.GetRegionByCodesAsync(regionId);
                query = query.Where(r => r.RegionId == region.Id);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(r => r.UserId == userId);
            }

            var entities = await query.ToListAsync();

            return Ok(entities.Select(e => new RegionMembership()
            {
                Id = e.Id,
                RegionId = e.RegionId,
                UserId = e.UserId,
                Role = (RegionMembershipRole)e.Role
            }));
        }

        [HttpGet("my")]
        public async Task<IActionResult> QueryMy(
            [FromQuery] string regionId)
        {
            return await Query(
                regionId: regionId,
                userId: User.GetUserId());
        }

        [HttpPost("")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _coreDbContext.RegionRoles.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var region = await _coreDbContext.GetRegionByIdAsync(entity.RegionId);
            var canDeleteMembership = await IsAuthorizedAsync(region, RegionRequirement.DeleteMembership((RegionMembershipRole)entity.Role));
            if (!canDeleteMembership)
            {
                return Unauthorized();
            }

            _coreDbContext.RegionRoles.Remove(entity);
            await _coreDbContext.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> IsAuthorizedAsync(RegionEntity region, RegionRequirement requirement)
            => (await _authorizationService.AuthorizeAsync(User, region, requirement)).Succeeded;
    }
}
