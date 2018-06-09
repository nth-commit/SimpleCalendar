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
            var result = await _eventService.GetEventAsync(id);
            switch (result.Status)
            {
                case EventGetResult.EventGetResultStatus.NotFound:
                case EventGetResult.EventGetResultStatus.Unauthorized:
                    return NotFound();
                case EventGetResult.EventGetResultStatus.Success:
                    return Ok(result);
                default:
                    throw new Exception("Unrecognised value");
            }
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] EventCreate create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.CreateEventAsync(create);
            if (result.Status == EventCreateResult.EventCreateResultStatus.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                return CreatedAtAction(
                    nameof(Get),
                    new { id = result.Event.Id },
                    result.Event);
            }

        }
    }
}
