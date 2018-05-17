using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions.Authorization;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventService
    {
        private readonly IMapper _mapper;
        private readonly CoreDbContext _coreDbContext;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public EventService(
            IMapper mapper,
            CoreDbContext coreDbContext,
            IUserAuthorizationService userAuthorizationService)
        {
            _mapper = mapper;
            _coreDbContext = coreDbContext;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task<Event> CreateEventAsync(EventCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);

            var region = await _coreDbContext.GetRegionByCodesAsync(create.RegionId.ToLower().Split('.'));
            if (region == null)
            {
                throw new ArgumentNullException(nameof(EventCreate.RegionId));
            }
            await _userAuthorizationService.AssertAuthorizedAsync(region, new CreateEventsRequirement());

            var ev = _mapper.Map<Event>(create);
            if (dryRun)
            {
                return ev;
            }
            else
            {
                return await Task.FromResult(new Event());
            }
        }
    }
}
