using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Data
{
    [Flags]
    public enum RegionPermission
    {
        Events_Read = 1 << 0,
        Events_Write = 1 << 1,
        Events_Write_Draft = 1 << 2,
        Events_Write_Publish = 1 << 6,

        Events_All = Events_Read | Events_Write | Events_Write_Draft | Events_Write_Publish,

        Memberships_Write_Reader = 1 << 3,
        Memberships_Write_Writer = 1 << 4,
        Memberships_Read = 1 << 5,

        All =
            Events_Read | Events_Write | Events_Write_Draft |
            Memberships_Write_Reader | Memberships_Write_Writer | Memberships_Read,

        Writer = Events_Write | Events_Write_Publish | Memberships_Write_Reader | Memberships_Write_Writer,

        None = 0
    }
}
