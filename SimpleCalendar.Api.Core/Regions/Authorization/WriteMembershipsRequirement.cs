using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions.Authorization
{
    public class WriteMembershipsRequirement : RegionRequirement
    {
        public RegionMembershipRole Role { get; set; }
    }
}
