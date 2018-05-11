using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<EventCreate, Event>()
                .ForMember(dest => dest.Id, opts => opts.ResolveUsing(src => Guid.NewGuid()));

            CreateMap<Event, EventTableEntity>()
                .ForMember(dest => dest.RowKey, opts => opts.ResolveUsing(src => src.Id))
                .ForMember(dest => dest.PartitionKey, opts => opts.ResolveUsing(src => src.RegionId))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)));

            CreateMap<EventTableEntity, Event>()
                .ConvertUsing((src, dest) => JsonConvert.DeserializeObject<Event>(src.DataJson));
        }
    }
}
