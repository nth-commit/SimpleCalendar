using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.RegionRoles.Impl.Query
{
    public class QueryRegionRoleCommand : IQueryRegionRoleCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public QueryRegionRoleCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context)
        {
            if (!await _authorizationService.CanQueryRolesAsync())
            {
                return new UnauthorizedResult();
            }

            var entities = await _coreDbContext.RegionRoles.ToListAsync();
            return new OkObjectResult(entities.Select(e => _mapper.Map<RegionRole>(e)));
        }
    }
}
