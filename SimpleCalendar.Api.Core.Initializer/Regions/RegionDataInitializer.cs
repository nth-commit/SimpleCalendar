using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Initializer.Regions
{
    public class RegionDataInitializer : IDataInitializer
    {
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public RegionDataInitializer(
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        public async Task RunAsync()
        {
            await _coreDbContext.Database.EnsureCreatedAsync();
        }
    }
}
