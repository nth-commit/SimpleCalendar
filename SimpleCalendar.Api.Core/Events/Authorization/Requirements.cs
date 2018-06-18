using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Events.Authorization
{
    public static class Requirements
    {
        public static ViewEventRequirement View => new ViewEventRequirement();
    }
}
