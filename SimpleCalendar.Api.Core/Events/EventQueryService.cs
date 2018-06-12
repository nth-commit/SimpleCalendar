using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
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
        private readonly IMapper _mapper;

        public EventQueryService(
            CoreDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventResult>> QueryEventsAsync(EventQuery query)
        {
            var events = await _dbContext.Events
                .Include(e => e.Region)
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            return events.Select(e => _mapper.MapEntityToResult(e, e.Region));
        }
    }
}
