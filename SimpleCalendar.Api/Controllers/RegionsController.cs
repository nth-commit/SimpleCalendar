using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Core.Data;
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
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public RegionsController(
            RegionService regionService,
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _regionService = regionService;
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> List([FromQuery] string parentId)
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

        [HttpGet("{*id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var region = id == "ROOT" ?
                await _coreDbContext.GetRegionByIdAsync(id) :
                await _coreDbContext.GetRegionByCodesAsync(id);

            if (region == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<RegionResult>(region));
            }
        }
    }
}
