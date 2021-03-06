﻿using System.ComponentModel.DataAnnotations;

namespace SimpleCalendar.Api.Models
{
    public class RegionMembershipCreate
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string RegionId { get; set; }

        [Required]
        public string RegionRoleId { get; set; }
    }
}
