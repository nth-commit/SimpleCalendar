using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.Regions.Impl.Create
{
    public class CreateRegionCommand : ICreateRegionCommand
    {
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly CoreDbContext _coreDbContext;
        private readonly RegionMapper _regionMapper;

        public CreateRegionCommand(
            IUserAuthorizationService userAuthorizationService,
            CoreDbContext coreDbContext,
            RegionMapper regionMapper)
        {
            _userAuthorizationService = userAuthorizationService;
            _coreDbContext = coreDbContext;
            _regionMapper = regionMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, RegionCreate create)
        {
            RegionEntity parentEntity = null;
            if (string.IsNullOrEmpty(create.ParentId))
            {
                parentEntity = await _coreDbContext.GetRegionByIdAsync(Constants.RootRegionId);
            }
            else
            {
                parentEntity = await _coreDbContext.GetRegionByIdAsync(create.ParentId);
                if (parentEntity == null)
                {
                    return BadRequest(context, nameof(RegionCreate.ParentId), "Parent region not found");
                }

                var parentLevel = parentEntity.GetLevel();
                if (parentLevel >= 5)
                {
                    return BadRequest(context, nameof(RegionCreate.ParentId), "Max level region reached");
                }
            }

            var entity = _regionMapper.MapToEntity(create);
            entity.ParentId = parentEntity.Id;

            var existingEntity = _coreDbContext.GetRegionByIdAsync(entity.Id);
            if (existingEntity != null)
            {
                return BadRequest(
                    context,
                    $"{nameof(RegionCreate.ParentId)}+{nameof(RegionCreate.Name)}",
                    $"Region with id {entity.Id} already exists");
            }

            await _coreDbContext.Regions.AddAsync(entity);
            await _coreDbContext.SaveChangesAsync();

            return new CreatedAtActionResult(
                "Get",
                "Regions",
                entity.Id,
                _regionMapper.MapToResult(entity));
        }

        private IActionResult BadRequest(ActionContext context, string propertyName, string errorMessage)
        {
            context.ModelState.AddModelError(propertyName, errorMessage);
            return new BadRequestObjectResult(context.ModelState);
        }
    }
}
