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

        [InverseProperty(nameof(RegionMembershipEntity.Region))]
        public List<RegionMembershipEntity> Memberships { get; set; }
    }

    public static class RegionEntityExtensions
    {
        public static int GetLevel(this RegionEntity region) =>
            region.Id == Constants.RootRegionId ? 0 : region.GetId().Split('/').Count();

        public static string GetId(this RegionEntity region) =>
            region.Id == Constants.RootRegionId ? Constants.RootRegionId : region.GetIdInternal();

        private static string GetIdInternal(this RegionEntity region)
        {
            if (region.ParentId == Constants.RootRegionId)
            {
                return region.Code;
            }
            else
            {
                var parentId = region.Parent.GetIdInternal();
                return string.IsNullOrEmpty(parentId) ? region.Code : $"{parentId}/{region.Code}";
            }
        }

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

        public static bool IsSuperAdministrator(this RegionEntity region, string userId) => region.IsInRole(userId, Constants.RegionRoles.SuperAdministrator);

        public static bool IsUser(this RegionEntity region, string userId) => region.IsInRole(userId, Constants.RegionRoles.User);

        private static bool IsInRole(
            this RegionEntity region,
            string userId,
            string regionRoleId)
        {
            if (region.Memberships == null)
            {
                throw new InvalidOperationException("Roles cannot be null to determine if user is in role");
            }

            return false;

            //return region.Memberships.Where(r => r.UserEmail == userId).Any(r => r.Role.HasFlag(role)) ||
            //    (region.Id != Constants.RootRegionId && IsInRole(region.Parent, userId, role));
        }
    }
}
