using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using SimpleCalendar.Framework;

namespace SimpleCalendar.Api.Commands.Events.Impl.Create
{
    public class CreateEventsCommand : ICreateEventsCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;
        private readonly IRegionCache _regionCache;
        private readonly EventMapper _mapper;

        public CreateEventsCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext,
            IRegionCache regionCache,
            EventMapper mapper)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
            _regionCache = regionCache;
            _mapper = mapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, EventInput create, EventInputOptions options)
        {
            var region = await _regionCache.GetRegionAsync(create.RegionId);
            if (region == null)
            {
                return new BadRequestObjectResult(nameof(EventInput.RegionId));
            }

            var canCreate = await _authorizationService.CanCreateEventsAsync(region);
            if (!canCreate)
            {
                return new UnauthorizedResult();
            }

            var ev = _mapper.MapToEvent(create);
            ev.Created = DateTime.UtcNow;
            ev.CreatedByEmail = context.HttpContext.User.GetUserEmail();

            var canPublish = await _authorizationService.CanPublishEventsAsync(region);
            if (canPublish)
            {
                ev.Published = ev.Created;
                ev.PublishedByEmail = ev.CreatedByEmail;
            }

            if (options.DryRun)
            {
                return new OkObjectResult(_mapper.MapToResult(ev));
            }
            else
            {
                var entity = _mapper.MapToEntity(ev);
                entity.RegionId = region.Id;
                entity.IsPublic = true; // TODO: Privacy

                await _coreDbContext.Events.AddAsync(entity);
                await _coreDbContext.SaveChangesAsync();

                return new CreatedAtActionResult(
                    "Get",
                    "Events",
                    new { id = entity.Id },
                    _mapper.MapToOutput(entity));
            }
        }
    }
}
