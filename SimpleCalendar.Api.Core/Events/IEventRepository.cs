using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Events
{
    public interface IEventRepository
    {
        Task<Event> CreateEventAsync(Event ev);
    }
}
