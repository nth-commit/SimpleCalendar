using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.Authorization;
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
        protected readonly IUserAuthorizationService _authorizationService;
        protected readonly CoreDbContext _coreDbContext;

        public RegionMembershipsController(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> Query(
            [FromQuery] string regionId,
            [FromQuery] string userEmail)
        {
            var canQuery = await _authorizationService.CanQueryMembershipsAsync(regionId, userEmail);
            if (!canQuery)
            {
                return Unauthorized();
            }

            var query = _coreDbContext.RegionMemberships.AsQueryable();
            var isRegionQuery = !string.IsNullOrWhiteSpace(regionId);

            RegionEntity region = null;
            if (isRegionQuery)
            {
                region = regionId == Constants.RootRegionId ?
                    await _coreDbContext.GetRegionByIdAsync(regionId) :
                    await _coreDbContext.GetRegionByCodesAsync(regionId);

                if (region == null)
                {
                    ModelState.AddModelError(nameof(regionId), "Region could not be found");
                    return BadRequest(ModelState);
                }
                query = query.Where(r => r.RegionId == region.Id);
            }

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                query = query.Where(r => r.UserEmail == userEmail);
            }

            var entities = await query.ToListAsync();
            if (!entities.Any())
            {
                return Ok(Enumerable.Empty<RegionMembership>());
            }

            var regionIds = entities
                .Select(e => e.RegionId)
                .Distinct();

            Dictionary<string, RegionEntity> regionsById = null;
            if (isRegionQuery)
            {
                regionsById = regionIds.ToDictionary(r => r, r => region);
            }
            else
            {
                var regions = await Task.WhenAll(regionIds.Select(r => _coreDbContext.GetRegionByIdAsync(r)));
                regionsById = regions.ToDictionary(r => r.Id);
            }

            return Ok(entities.Select(e => new RegionMembership()
            {
                Id = e.Id,
                RegionId = regionsById[e.RegionId].GetId(),
                UserId = e.UserEmail,
                RegionRoleId = e.RegionRoleId
            }));
        }

        [HttpGet("my")]
        public async Task<IActionResult> QueryMy(
            [FromQuery] string regionId)
        {
            return await Query(
                regionId: regionId,
                userEmail: User.GetUserEmail());
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

            var canCreate = await _authorizationService.CanCreateMembershipAsync(region, create.RegionRoleId);
            if (!canCreate)
            {
                return Unauthorized();
            }

            var roleExists = await _coreDbContext.RegionMemberships
                .Where(r => r.RegionId == region.Id)
                .Where(r => r.UserEmail == create.UserEmail)
                .AnyAsync();

            if (roleExists)
            {
                ModelState.AddModelError(
                    $"{nameof(RegionMembershipCreate.RegionId)}+{nameof(RegionMembershipCreate.UserEmail)}",
                    "Region not found");
                return BadRequest(ModelState);
            }

            // TODO: Validate region role exists

            var result = await _coreDbContext.RegionMemberships.AddAsync(new RegionMembershipEntity()
            {
                UserEmail = create.UserEmail,
                RegionId = region.Id,
                RegionRoleId = create.RegionRoleId 
            });

            return Created(
                "",
                new RegionMembership()
                {
                    Id = result.Entity.Id,
                    UserId = result.Entity.UserEmail,
                    RegionId = create.RegionId,
                    RegionRoleId = create.RegionRoleId
                });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _coreDbContext.RegionMemberships.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            var region = await _coreDbContext.GetRegionByIdAsync(entity.RegionId);
            var canDelete = await _authorizationService.CanDeleteMembershipsAsync(region);
            if (!canDelete)
            {
                return Unauthorized();
            }

            _coreDbContext.RegionMemberships.Remove(entity);
            await _coreDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
