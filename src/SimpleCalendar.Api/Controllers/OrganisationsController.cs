using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Controllers
{
    [Route("organisations")]
    public class OrganisationsController : Controller
    {
        private readonly OrganisationService _organisationService;

        public OrganisationsController(
            OrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> List(
            [FromQuery] bool isMember,
            [FromQuery] string region)
        {
            var result = await _organisationService.QueryAsync(isMember, region);
            return Ok(result);
        }
    }
}
