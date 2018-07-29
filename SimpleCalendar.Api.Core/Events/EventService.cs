using AutoMapper;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Framework.Identity;

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

            var canView = await _userAuthorizationService.CanViewEventAsync(entity);
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

            var region = await _coreDbContext.GetRegionByIdAsync(create.RegionId);
            if (region == null)
            {
                throw new ArgumentNullException(nameof(EventCreate.RegionId));
            }

            var canCreate = await _userAuthorizationService.CanCreateEventsAsync(region);
            if (!canCreate)
            {
                return EventCreateResult.Unauthorized;
            }

            var ev = _mapper.Map<Event>(create);
            ev.Created = DateTime.UtcNow;
            ev.CreatedByEmail = _userAccessor.User.GetUserEmail();

            var canPublish = await _userAuthorizationService.CanPublishEventsAsync(region);
            if (canPublish)
            {
                ev.Published = ev.Created;
                ev.PublishedById = ev.CreatedByEmail;
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
