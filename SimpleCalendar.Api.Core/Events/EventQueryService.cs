using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Events.Authorization;
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
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly IMapper _mapper;

        public EventQueryService(
            CoreDbContext dbContext,
            IUserAuthorizationService userAuthorizationService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userAuthorizationService = userAuthorizationService;
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

            // TODO: Make authorization check concurrent?
            return events
                .Where(e => _userAuthorizationService.IsAuthorizedAsync(e, Requirements.View).Result)
                .Select(e => _mapper.MapEntityToResult(e, e.Region));
        }
    }
}
