using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Commands.Events;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Api.Services;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly Lazy<IQueryEventsCommand> _queryEventsCommand;
        private readonly Lazy<IGetEventsCommand> _getEventsCommand;
        private readonly Lazy<ICreateEventsCommand> _createEventsCommand;
        private readonly IDateTimeAccessor _dateTimeAccessor;

        public EventsController(
            Lazy<IQueryEventsCommand> queryEventsCommand,
            Lazy<IGetEventsCommand> getEventsCommand,
            Lazy<ICreateEventsCommand> createEventsCommand,
            IDateTimeAccessor dateTimeAccessor)
        {
            _queryEventsCommand = queryEventsCommand;
            _getEventsCommand = getEventsCommand;
            _createEventsCommand = createEventsCommand;
            _dateTimeAccessor = dateTimeAccessor;
        }

        [HttpGet("")]
        public Task<IActionResult> Query(
            [FromQuery][Required]string regionId,
            [FromQuery]bool? inherit,
            [FromQuery]DateTime? from,
            [FromQuery]DateTime? to) =>
                _queryEventsCommand.Value.InvokeAsync(
                    ControllerContext,
                    CreateQuery(
                        regionId: regionId,
                        inherit: inherit,
                        from: from,
                        to: to));

        [HttpGet("today")]
        public Task<IActionResult> QueryToday(
            [FromQuery][Required]string regionId,
            [FromQuery]bool? inherit,
            [FromQuery][Timezone]string timezone = null) =>
                _queryEventsCommand.Value.InvokeAsync(
                    ControllerContext,
                    CreateQuery(
                        regionId: regionId,
                        inherit: inherit,
                        from: GetTodayUtc(timezone)));

        [HttpGet("my")]
        public Task<IActionResult> QueryMy([FromQuery][Required]string regionId) =>
            _queryEventsCommand.Value.InvokeAsync(
                ControllerContext,
                CreateQuery(
                    regionId: regionId,
                    userEmail: User.GetUserEmail()));

        [HttpGet("{id}")]
        public Task<IActionResult> Get([FromRoute] string id) =>
            _getEventsCommand.Value.InvokeAsync(ControllerContext, id);

        [Authorize]
        [HttpPost("")]
        public Task<IActionResult> Create(
            [FromBody][Required]EventInput create,
            [FromQuery]bool dryRun = false) =>
                _createEventsCommand.Value.InvokeAsync(ControllerContext, create, new EventInputOptions()
                {
                    DryRun = dryRun
                });

        private EventQuery CreateQuery(
            string regionId,
            string userEmail = null,
            bool? inherit = null,
            DateTime? from = null,
            DateTime? to = null) =>
                new EventQuery()
                {
                    RegionId = regionId,
                    UserEmail = userEmail,
                    Inherit = inherit ?? true,
                    From = from ?? DateTime.MinValue,
                    To = to ?? DateTime.MaxValue
                };

        private DateTime GetTodayUtc(string timezone)
        {
            if (string.IsNullOrEmpty(timezone))
            {
                return _dateTimeAccessor.UtcNow;
            }

            var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var utcOffset = timezoneInfo.GetUtcOffset(_dateTimeAccessor.UtcNow);
            return _dateTimeAccessor.UtcNow.Subtract(utcOffset);
        }
    }
}
