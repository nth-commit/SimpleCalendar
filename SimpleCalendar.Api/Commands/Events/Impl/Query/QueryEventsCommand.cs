using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.Events.Impl.Query
{
    public class QueryEventsCommand : IQueryEventsCommand
    {
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly CoreDbContext _dbContext;
        private readonly IRegionCache _regionCache;
        private readonly EventMapper _eventMapper;

        public QueryEventsCommand(
            IUserAuthorizationService userAuthorizationService,
            CoreDbContext dbContext,
            IRegionCache regionCache,
            EventMapper eventMapper)
        {
            _userAuthorizationService = userAuthorizationService;
            _dbContext = dbContext;
            _regionCache = regionCache;
            _eventMapper = eventMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, EventQuery query)
        {
            if (query.From > query.To)
            {
                throw new Exception("Failed");
            }

            var isQueryingUser = !string.IsNullOrEmpty(query.UserEmail);

            var dbQuery = _dbContext.Events
                .Where(e => !e.IsDeleted)
                .Where(e =>
                    (query.From <= e.StartTime && e.StartTime <= query.To) ||
                    (query.From <= e.EndTime && e.EndTime <= query.To) ||
                    (e.StartTime < query.From && query.To < e.EndTime))
                .Where(e => !isQueryingUser || e.CreatedByEmail == query.UserEmail);

            var events = await dbQuery.ToListAsync();
            foreach (var ev in events)
            {
                await PrepareEventAsync(ev);
            }

            return new OkObjectResult(events
                .Where(e => _userAuthorizationService.CanViewEventAsync(e).Result)
                .Select(e => _eventMapper.MapToOutput(e)));
        }

        private async Task PrepareEventAsync(EventEntity ev)
        {
            ev.Region = await _regionCache.GetRegionAsync(ev.RegionId);
        }
    }
}
