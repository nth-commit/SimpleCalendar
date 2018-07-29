using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SimpleCalendar.Api.Commands.Regions;
using SimpleCalendar.Api.Core.Data;
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
        private readonly Lazy<IQueryRegionCommand> _queryRegionCommand;
        private readonly Lazy<IGetRegionCommand> _getRegionCommand;
        private readonly Lazy<ICreateRegionCommand> _createRegionCommand;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RegionsController(
            Lazy<IQueryRegionCommand> queryRegionCommand,
            Lazy<IGetRegionCommand> getRegionCommand,
            Lazy<ICreateRegionCommand> createRegionCommand,
            IHostingEnvironment hostingEnvironment)
        {
            _queryRegionCommand = queryRegionCommand;
            _getRegionCommand = getRegionCommand;
            _createRegionCommand = createRegionCommand;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("")]
        public Task<IActionResult> Query([FromQuery] string parentId = Constants.RootRegionId) =>
            _queryRegionCommand.Value.InvokeAsync(ControllerContext, parentId);

        [HttpGet("{*id}")]
        public Task<IActionResult> Get([FromRoute] string id) =>
            _getRegionCommand.Value.InvokeAsync(ControllerContext, id);

        [HttpPost]
        public Task<IActionResult> Create([FromBody] Models.RegionCreate create)
        {
            if (!_hostingEnvironment.IsUnitTest())
            {
                return Task.FromResult((IActionResult)NotFound());
            }
            return _createRegionCommand.Value.InvokeAsync(ControllerContext, create);
        }
    }
}
