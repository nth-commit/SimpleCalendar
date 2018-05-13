using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("events")]
    public class EventsController : Controller
    {

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
    }
}
