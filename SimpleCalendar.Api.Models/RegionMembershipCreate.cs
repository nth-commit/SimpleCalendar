using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
