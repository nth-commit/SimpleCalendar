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
        Events_WriteDraft = 1 << 2,

        Events_All = Events_Read | Events_Write | Events_WriteDraft,

        Memberships_WriteReader = 1 << 3,
        Memberships_WriteWriter = 1 << 4,
        Memberships_Read = 1 << 5,

        All =
            Events_Read | Events_Write | Events_WriteDraft |
            Memberships_WriteReader | Memberships_WriteWriter | Memberships_Read,

        Writer = Events_Write | Memberships_WriteReader | Memberships_WriteWriter,

        None = 0
    }
}
