using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands.Regions.Impl.Query
{
    public class QueryRegionCommand : IQueryRegionCommand
    {
        private readonly CoreDbContext _coreDbContext;
        private readonly RegionMapper _regionMapper;

        public QueryRegionCommand(
            CoreDbContext coreDbContext,
            RegionMapper regionMapper)
        {
            _coreDbContext = coreDbContext;
            _regionMapper = regionMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string parentRegionId)
        {
            var region = await _coreDbContext.GetRegionByIdAsync(parentRegionId, includeChildren: true);
            if (region == null)
            {
                context.ModelState.AddModelError(nameof(parentRegionId), $"Could not find region with {nameof(parentRegionId)} \"{parentRegionId}\"");
                return new BadRequestObjectResult(context.ModelState);
            }
            return new OkObjectResult(region.Children.Select(r => _regionMapper.MapToResult(r)));
        }
    }
}
