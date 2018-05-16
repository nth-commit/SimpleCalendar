using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventService
    {
        private readonly IMapper _mapper;
        private readonly CoreDbContext _coreDbContext;
        private readonly IClaimsPrincipalAuthorizationService _claimsPrincipalAuthorizationService;
        private readonly IAuthorizationService _authorizationService;

        public EventService(
            IMapper mapper,
            CoreDbContext coreDbContext,
            IClaimsPrincipalAuthorizationService claimsPrincipalAuthorizationService,
            IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _coreDbContext = coreDbContext;
            _claimsPrincipalAuthorizationService = claimsPrincipalAuthorizationService;
            _authorizationService = authorizationService;
        }

        public async Task<Event> CreateEventAsync(EventCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);

            var region = await _coreDbContext.GetRegionByCodesAsync(create.RegionId.ToLower().Split('.'));
            //_authorizationService.AuthorizeAsync()

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
