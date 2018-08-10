using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Models
{
    public class Region
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Timezone { get; set; } = "New Zealand Standard Time"; // TODO: Save in DB

        public RegionAuthorization Permissions { get; set; }
    }
}
