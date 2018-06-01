using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("regions")]
    public class RegionsController : Controller
    {
        [HttpGet]
        public Task<IActionResult> List(
            [FromQuery] string parentId)
        {
            throw new NotImplementedException();
        }
    }
}
