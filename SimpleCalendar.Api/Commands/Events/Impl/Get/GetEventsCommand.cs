using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Utility.Authorization;

namespace SimpleCalendar.Api.Commands.Events.Impl.Get
{
    public class GetEventsCommand : IGetEventsCommand
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _dbContext;
        private readonly IRegionCache _regionCache;
        private readonly EventMapper _eventMapper;

        public GetEventsCommand(
            IUserAuthorizationService authorizationService,
            CoreDbContext dbContext,
            IRegionCache regionCache,
            EventMapper eventMapper)
        {
            _authorizationService = authorizationService;
            _dbContext = dbContext;
            _regionCache = regionCache;
            _eventMapper = eventMapper;
        }

        public async Task<IActionResult> InvokeAsync(ActionContext context, string eventId)
        {
            var entity = await _dbContext.GetEventByIdAsync(eventId);
            if (entity == null)
            {
                return new NotFoundResult();
            }

            var canView = await _authorizationService.CanViewEventAsync(entity);
            if (!canView)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(_eventMapper.MapToOutput(entity));
        }
    }
}
