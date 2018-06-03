using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Regions;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("regions")]
    public class RegionsController : Controller
    {
        private readonly RegionService _regionService;

        public RegionsController(RegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] string parentId)
        {
            try
            {
                var result = await _regionService.ListRegionsAsync(parentId);
                return Ok(result);
            }
            catch (ClientValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
