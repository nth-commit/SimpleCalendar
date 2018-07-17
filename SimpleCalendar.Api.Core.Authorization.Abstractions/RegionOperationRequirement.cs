using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionOperationRequirement : OperationAuthorizationRequirement
    {
        protected RegionOperationRequirement(string operationName) => Name = operationName;

        public static CreateMembershipRequirement CreateMembership(string regionRoleId) => new CreateMembershipRequirement(regionRoleId);
        public static RegionOperationRequirement ViewMemberships => new RegionOperationRequirement(nameof(ViewMemberships));
        public static RegionOperationRequirement DeleteMemberships => new RegionOperationRequirement(nameof(DeleteMemberships));

        public class CreateMembershipRequirement : RegionOperationRequirement
        {
            public string RegionRoleId { get; set; }

            public CreateMembershipRequirement(string regionRoleId)
                : base(nameof(CreateMembership)) =>
                    RegionRoleId = regionRoleId;
        }
    }
}
