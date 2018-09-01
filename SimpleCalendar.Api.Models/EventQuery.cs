using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Models
{
    public class EventQuery
    {
        public string RegionId { get; set; }

        public string UserEmail { get; set; }

        public bool Inherit { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
