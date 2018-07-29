using System.Collections.Generic;

namespace SimpleCalendar.Api.Models
{
    public class RegionAuthorization
    {
        public Dictionary<string, bool> CanAddMemberships { get; set; }
    }
}