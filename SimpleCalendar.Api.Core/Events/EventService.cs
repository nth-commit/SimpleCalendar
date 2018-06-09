using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Core.Regions.Authorization;
using SimpleCalendar.Framework.Identity;
using SimpleCalendar.Api.Core.Events.Authorization;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventService
    {
        private readonly IMapper _mapper;
        private readonly CoreDbContext _coreDbContext;
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly IUserAccessor _userAccessor;

        public EventService(
            IMapper mapper,
            CoreDbContext coreDbContext,
            IUserAuthorizationService userAuthorizationService,
            IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _coreDbContext = coreDbContext;
            _userAuthorizationService = userAuthorizationService;
            _userAccessor = userAccessor;
        }

        public async Task<EventGetResult> GetEventAsync(string id)
        {
            var entity = await _coreDbContext.GetEventByIdAsync(id);
            if (entity == null)
            {
                return EventGetResult.NotFound;
            }

            var canView = (await _userAuthorizationService.AuthorizeAsync(entity, new ViewEventRequirement())).Succeeded;
            if (!canView)
            {
                return EventGetResult.Unauthorized;
            }

            return EventGetResult.Success(_mapper.MapEntityToResult(entity, entity.Region));
        }

        public async Task<EventCreateResult> CreateEventAsync(EventCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);

            var region = await _coreDbContext.GetRegionByCodesAsync(create.RegionId.ToLower().Split('.'));
            if (region == null)
            {
                throw new ArgumentNullException(nameof(EventCreate.RegionId));
            }

            var canCreate = await _userAuthorizationService.IsAuthorizedAsync(region, new CreateEventsRequirement());
            if (!canCreate)
            {
                return EventCreateResult.Unauthorized;
            }

            var ev = _mapper.Map<Event>(create);
            ev.Created = DateTime.UtcNow;
            ev.CreatedById = _userAccessor.User.GetUserId();

            var canPublish = await _userAuthorizationService.IsAuthorizedAsync(region, new PublishEventsRequirement());
            if (canPublish)
            {
                ev.Published = ev.Created;
                ev.PublishedById = ev.CreatedById;
            }

            if (dryRun)
            {
                return EventCreateResult.Success(_mapper.Map<EventResult>(ev), canPublish);
            }
            else
            {
                var entity = _mapper.Map<EventEntity>(ev);
                entity.RegionId = region.Id;

                await _coreDbContext.Events.AddAsync(entity);
                await _coreDbContext.SaveChangesAsync();

                return EventCreateResult.Success(_mapper.MapEntityToResult(entity, region), canPublish);
            }
        }
    }
}
