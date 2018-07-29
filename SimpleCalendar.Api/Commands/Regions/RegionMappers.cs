using Newtonsoft.Json;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapper
{
    public static class RegionMappers
    {
        public static void AddRegionMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.CreateMap<RegionCreate, RegionEntity>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.ResolveUsing(src => GenerateRegionId(src)))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)))
                .ForMember(dest => dest.DataJsonVersion, opts => opts.UseValue(1));

            conf.CreateMap<RegionEntity, Region>()
                .ConstructUsing(src => string.IsNullOrEmpty(src.DataJson) ?
                    new Region() { Id = src.Id, Name = src.Id } :
                    JsonConvert.DeserializeObject<Region>(src.DataJson));
        }

        private static string GenerateRegionId(RegionCreate create)
        {
            var id = string.Empty;
            if (!string.IsNullOrEmpty(create.ParentId))
            {
                id += create.ParentId + "/";
            }
            id += create.Name.ToLower().Replace(' ', '-');
            return id;
        }
    }
}
