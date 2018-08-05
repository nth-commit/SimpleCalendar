using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SimpleCalendar.Api.Commands;
using SimpleCalendar.Api.Commands.Events;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly Lazy<IQueryEventsCommand> _queryEventsCommand;
        private readonly Lazy<IGetEventsCommand> _getEventsCommand;
        private readonly Lazy<ICreateEventsCommand> _createEventsCommand;

        public EventsController(
            Lazy<IQueryEventsCommand> queryEventsCommand,
            Lazy<IGetEventsCommand> getEventsCommand,
            Lazy<ICreateEventsCommand> createEventsCommand)
        {
            _queryEventsCommand = queryEventsCommand;
            _getEventsCommand = getEventsCommand;
            _createEventsCommand = createEventsCommand;
        }

        [HttpGet("")]
        public Task<IActionResult> Query(
            [FromQuery][Required]string regionId,
            [FromQuery]bool? inherit,
            [FromQuery]DateTime? from,
            [FromQuery]DateTime? to) =>
                _queryEventsCommand.Value.InvokeAsync(ControllerContext, new EventQuery()
                {
                    RegionId = regionId,
                    Inherit = inherit ?? true,
                    From = from ?? DateTime.MinValue,
                    To = to ?? DateTime.MaxValue
                });

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
    }
}
