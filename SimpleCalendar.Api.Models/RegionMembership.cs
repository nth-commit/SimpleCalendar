﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Models
{
    public class RegionMembership
    {
        public string Id { get; set; }

        public string UserEmail { get; set; }

        public string RegionId { get; set; }

        public string RegionRoleId { get; set; }

        public RegionMembershipAuthorization Permissions { get; set; }
    }
}
