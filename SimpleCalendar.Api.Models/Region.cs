using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Models
{
    public class Region
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public RegionAuthorization Permissions { get; set; }
    }
}
