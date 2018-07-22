using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class RegionOperationRequirement : OperationAuthorizationRequirement
    {
        protected RegionOperationRequirement(string operationName) => Name = operationName;

        public static CreateMembershipRequirement CreateMembership(string regionRoleId) =>
            new CreateMembershipRequirement(regionRoleId);

        public static RegionOperationRequirement DeleteMemberships(string regionRoleId, string userEmail) =>
            new DeleteMembershipRequirement(regionRoleId, userEmail);

        public static RegionOperationRequirement ViewMemberships =>
            new RegionOperationRequirement(nameof(ViewMemberships));

        public static RegionOperationRequirement QueryMemberships =>
            new RegionOperationRequirement(nameof(QueryMemberships));

        public class DeleteMembershipRequirement : RegionOperationRequirement
        {
            public string RegionRoleId { get; set; }

            public string UserEmail { get; set; }

            public DeleteMembershipRequirement(string regionRoleId, string userEmail)
                : base(nameof(DeleteMemberships))
            {
                RegionRoleId = regionRoleId;
                UserEmail = userEmail;
            }
        }

        public class CreateMembershipRequirement : RegionOperationRequirement
        {
            public string RegionRoleId { get; set; }

            public CreateMembershipRequirement(string regionRoleId)
                : base(nameof(CreateMembership)) =>
                    RegionRoleId = regionRoleId;
        }
    }
}
