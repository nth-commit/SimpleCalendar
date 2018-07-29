using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.Regions.Impl.Get
{
    public class GetRegionCommand : IGetRegionCommand
    {
        private readonly IRegionCache _regionCache;
        private readonly RegionMapper _regionMapper;

        public GetRegionCommand(
            IRegionCache regionCache,
            RegionMapper regionMapper)
        {
            _regionCache = regionCache;
            _regionMapper = regionMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string regionId)
        {
            if (string.IsNullOrEmpty(regionId))
            {
                context.ModelState.AddModelError(nameof(regionId), "Region id was invalid");
                return new BadRequestObjectResult(context.ModelState);
            }

            var entity = await _regionCache.GetRegionAsync(regionId);
            if (entity == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(_regionMapper.MapToResult(entity));
        }
    }
}
