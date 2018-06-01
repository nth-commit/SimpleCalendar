using AutoMapper;
using SimpleCalendar.Api.Core.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Initializer.Regions
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<RegionCreate, RegionUpdate>();
        }
    }
}
