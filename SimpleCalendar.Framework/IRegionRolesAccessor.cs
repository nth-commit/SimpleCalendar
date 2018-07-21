using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Framework
{
    public interface IRegionRolesAccessor
    {
        IEnumerable<RegionRoleEntity> RegionRoles { get; }
    }
}
