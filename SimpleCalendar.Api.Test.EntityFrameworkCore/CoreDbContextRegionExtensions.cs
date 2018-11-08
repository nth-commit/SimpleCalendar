using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Data
{
    public static class CoreDbContextRegionExtensions
    {
        public static async Task<CoreDbContext> AddRegionAsync(
            this CoreDbContext coreDbContext, string parentId, string name)
        {
            var id = string.Empty;
            if (parentId != Constants.RootRegionId)
            {
                id += parentId + "/";
            }
            id += name.ToLower().Replace(' ', '-');

            await coreDbContext.Regions.AddAsync(new RegionEntity()
            {
                Id = id,
                ParentId = parentId,
                DataJson = JsonConvert.SerializeObject(new
                {
                    Name = name
                }),
                DataJsonVersion = 1
            });

            return coreDbContext;
        }

        public static async Task<CoreDbContext> AddRegionAsync(
            this Task<CoreDbContext> coreDbContextTask, string parentId, string name)
        {
            var coreDbContext = await coreDbContextTask;
            return await coreDbContext.AddRegionAsync(parentId, name);
        }
    }
}
