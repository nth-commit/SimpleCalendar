using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public class RegionEntity
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string ParentId { get; set; }

        public RegionEntity Parent { get; set; }

        public string DataJson { get; set; }

        public int DataJsonVersion { get; set; }

        [InverseProperty(nameof(Parent))]
        public List<RegionEntity> Children { get; set; }

        [InverseProperty(nameof(RegionRoleEntity.Region))]
        public List<RegionRoleEntity> Roles { get; set; }
    }

    public static class RegionEntityExtensions
    {

        public static IEnumerable<RegionEntity> GetRegions(
            this RegionEntity region,
            bool includeRoot = false)
        {
            if (!includeRoot && region.Id == Constants.RootRegionId)
            {
                return Enumerable.Empty<RegionEntity>();
            }

            var result = new List<RegionEntity>() { region };

            while ((includeRoot && region.Id != Constants.RootRegionId) || (!includeRoot && region.ParentId != Constants.RootRegionId))
            {
                if (region.Parent == null)
                {
                    throw new InvalidOperationException("Could not resolve all regions");
                }

                region = region.Parent;
                result.Insert(0, region);
            }

            return result;
        }
    }
}
