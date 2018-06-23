using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Models
{
    public class RegionMembership
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string RegionId { get; set; }

        public RegionMembershipRole Role { get; set; }
    }
}
