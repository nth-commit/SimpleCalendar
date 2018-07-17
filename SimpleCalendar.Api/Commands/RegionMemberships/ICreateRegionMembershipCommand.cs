using SimpleCalendar.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands.RegionMemberships
{
    public interface ICreateRegionMembershipCommand : ICommand<RegionMembershipCreate>
    {
    }
}
