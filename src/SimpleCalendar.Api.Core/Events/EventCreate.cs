using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventCreate
    {
        public string RegionId { get; set; }

        public string Name { get; set; }
    }
}
