using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Authorization;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Utility.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventQueryService : IEventQueryService
    {
        private readonly CoreDbContext _dbContext;
        private readonly IUserAccessor _userAccessor;
        private readonly IEventPermissionResolver _eventPermissionResolver;
        private readonly IMapper _mapper;

        public EventQueryService(
            CoreDbContext dbContext,
            IUserAccessor userAccessor,
            IEventPermissionResolver eventPermissionResolver,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userAccessor = userAccessor;
            _eventPermissionResolver = eventPermissionResolver;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventResult>> QueryEventsAsync(EventQuery query)
        {
            if (query.From > query.To)
            {
                throw new Exception("Failed");
            }

            var dbQuery = _dbContext.Events
                .IncludeRegionHierarchy()
                .Where(e => !e.IsDeleted);

            if (query.From > DateTime.MinValue)
            {
                dbQuery = dbQuery.Where(e => e.StartTime >= query.From);
            }

            if (query.To < DateTime.MaxValue)
            {
                dbQuery = dbQuery.Where(e => e.EndTime <= query.To);
            }

            var events = await dbQuery.ToListAsync();

            var lazyRegionRolesTask = new Lazy<Task<IEnumerable<RegionRoleEntity>>>(
                async () => await _dbContext.RegionRoles.ToListAsync(),
                isThreadSafe: true);

            var lazyRegionMembershipsTask = new Lazy<Task<IEnumerable<RegionMembershipEntity>>>(
                async () => await _dbContext.RegionMemberships
                    .Where(rm => rm.UserEmail == _userAccessor.User.GetUserEmail())
                    .ToListAsync(),
                isThreadSafe: true);

            return events
                .Where(e => _eventPermissionResolver.HasPermissionAsync(
                    EventPermissions.View,
                    e,
                    _userAccessor.User,
                    lazyRegionRolesTask,
                    lazyRegionMembershipsTask).Result)
                .Select(e => _mapper.MapEntityToResult(e, e.Region));
        }
    }
}
