using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly EventService _eventService;

        public EventsController(
            EventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize]
        [HttpGet("")]
        public IActionResult List()
        {
            return Ok(new object[]
            {
                new
                {
                    Name = "Test event"
                }
            });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _eventService.GetEventAsync(id));
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] EventCreate create)
        {
            var result = await _eventService.CreateEventAsync(create);
            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                result);
        }
    }
}
