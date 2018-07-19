using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Authorization
{
    public class EventPermissionRequirement : IAuthorizationRequirement
    {
        public EventPermissions Permission { get; private set; }

        public static EventPermissionRequirement View => new EventPermissionRequirement()
        {
            Permission = EventPermissions.View
        };
    }
}
