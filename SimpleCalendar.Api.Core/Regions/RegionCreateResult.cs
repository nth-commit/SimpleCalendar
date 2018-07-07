using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionCreateResult
    {
        public enum RegionCreateResultStatus
        {
            Success,
            ParentRegionNotFound,
            RegionAlreadyExists,
            MaxRegionLevelReached
        }

        public RegionCreateResultStatus Status { get; private set; }

        public RegionEntity Result { get; private set; }

        public bool IsSuccessful => Status == RegionCreateResultStatus.Success;

        public static RegionCreateResult Success(RegionEntity result) => Create(RegionCreateResultStatus.Success, result);
        public static RegionCreateResult ParentRegionNotFound => Create(RegionCreateResultStatus.ParentRegionNotFound);
        public static RegionCreateResult RegionAlreadyExists => Create(RegionCreateResultStatus.RegionAlreadyExists);
        public static RegionCreateResult MaxRegionLevelReached => Create(RegionCreateResultStatus.MaxRegionLevelReached);

        private static RegionCreateResult Create(RegionCreateResultStatus status, RegionEntity result = null) => new RegionCreateResult()
        {
            Status = status,
            Result = result
        };
    }
}
