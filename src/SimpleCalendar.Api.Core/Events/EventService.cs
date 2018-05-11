using AutoMapper;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventService
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public EventService(
            IMapper mapper,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<Event> CreateEventAsync(EventCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);

            var ev = _mapper.Map<Event>(create);
            if (dryRun)
            {
                return ev;
            }
            else
            {
                return await _eventRepository.CreateEventAsync(ev);
            }
        }
    }
}
