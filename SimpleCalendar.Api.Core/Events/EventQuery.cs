using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class EventQuery
    {
        public string RegionId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
