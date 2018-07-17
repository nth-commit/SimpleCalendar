using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SimpleCalendar.Api.Core.Data
{
    public class UserEntity
    {
        [Key]
        public string Email { get; set; }

        public string ClaimsByAuthorityJson { get; set; }

        public int ClaimsByAuthorityVersion { get; set; }

        [InverseProperty(nameof(RegionMembershipEntity.User))]
        public List<RegionMembershipEntity> Memberships { get; set; }
    }
}
