﻿using AutoMapper;
using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<RegionCreate, RegionEntity>()
                .ForMember(dest => dest.Code, opts => opts.ResolveUsing(src => src.Name.Replace(' ', '_').ToLowerInvariant()))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.DataJsonVersion, opts => opts.UseValue(1));

            CreateMap<RegionUpdate, RegionEntity>()
                .ForMember(dest => dest.Code, opts => opts.ResolveUsing(src => src.Name.Replace(' ', '_').ToLowerInvariant()))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.DataJsonVersion, opts => opts.UseValue(1));
        }
    }
}