using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands.Regions.Impl.Query
{
    public class QueryRegionCommand : IQueryRegionCommand
    {
        private readonly IRegionCache _regionCache;
        private readonly RegionMapper _regionMapper;

        public QueryRegionCommand(
            IRegionCache regionCache,
            RegionMapper regionMapper)
        {
            _regionCache = regionCache;
            _regionMapper = regionMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string parentRegionId)
        {
            var region = await _regionCache.GetRegionAsync(parentRegionId);
            if (region == null)
            {
                context.ModelState.AddModelError(nameof(parentRegionId), $"Could not find region with {nameof(parentRegionId)} \"{parentRegionId}\"");
                return new BadRequestObjectResult(context.ModelState);
            }

            var childRegions = await _regionCache.GetRegionsByParentAsync(parentRegionId);
            return new OkObjectResult(childRegions.Select(r => _regionMapper.MapToResult(r)));
        }
    }
}
