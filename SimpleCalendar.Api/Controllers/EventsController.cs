﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IEventQueryService _eventQueryService;

        public EventsController(
            EventService eventService,
            IEventQueryService eventQueryService,
            IServiceProvider serviceProvider)
        {
            _eventService = eventService;
            _eventQueryService = eventQueryService;
        }

        [HttpGet("")]
        public async Task<IActionResult> List(
            [FromQuery] string regionId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            return Ok(await _eventQueryService.QueryEventsAsync(new EventQuery()
            {
                RegionId = regionId,
                From = from ?? DateTime.MinValue,
                To = to ?? DateTime.MaxValue
            }));
        }

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
