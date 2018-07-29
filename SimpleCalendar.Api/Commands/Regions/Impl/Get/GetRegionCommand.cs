using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.Regions.Impl.Get
{
    public class GetRegionCommand : IGetRegionCommand
    {
        private readonly CoreDbContext _coreDbContext;
        private readonly RegionMapper _regionMapper;

        public GetRegionCommand(
            CoreDbContext coreDbContext,
            RegionMapper regionMapper)
        {
            _coreDbContext = coreDbContext;
            _regionMapper = regionMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string regionId)
        {
            if (string.IsNullOrEmpty(regionId))
            {
                context.ModelState.AddModelError(nameof(regionId), "Region id was invalid");
                return new BadRequestObjectResult(context.ModelState);
            }

            var entity = await _coreDbContext.GetRegionByIdAsync(regionId);
            if (entity == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(_regionMapper.MapToResult(entity));
        }
    }
}
