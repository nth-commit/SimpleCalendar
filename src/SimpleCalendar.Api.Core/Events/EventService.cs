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

        public async Task<EventResult> GetEventAsync(string id)
        {
            var entity = await _coreDbContext.GetEventByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            // TODO: Authorize user is creator or event is published and user is in region or event is public

            return _mapper.MapEntityToResult(entity, entity.Region);
        }

        public async Task<EventResult> CreateEventAsync(EventCreate create, bool dryRun = false)
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
            ev.Created = DateTime.UtcNow;
            ev.CreatedById = _userAccessor.User.Identity.Name;

            var canPublish = (await _userAuthorizationService.AuthorizeAsync(region, new PublishEventsRequirement())).Succeeded;
            if (canPublish)
            {
                ev.Published = ev.Created;
                ev.PublishedById = ev.CreatedById;
            }

            if (dryRun)
            {
                return _mapper.Map<EventResult>(ev);
            }
            else
            {
                var entity = _mapper.Map<EventEntity>(ev);
                entity.RegionId = region.Id;

                await _coreDbContext.Events.AddAsync(entity);
                await _coreDbContext.SaveChangesAsync();

                return _mapper.MapEntityToResult(entity, region);
            }
        }
    }
}
