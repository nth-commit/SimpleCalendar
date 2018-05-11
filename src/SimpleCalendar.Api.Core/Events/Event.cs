using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events
{
    public class Event
    {
        public string Id { get; set; }

        public string RegionId { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Created { get; set; }

        public string CreatedById { get; set; }

        public DateTime? Deleted { get; set; }

        public string DeletedById { get; set; }
    }
}
