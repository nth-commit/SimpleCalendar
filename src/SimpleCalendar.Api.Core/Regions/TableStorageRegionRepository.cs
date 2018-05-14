using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public class TableStorageRegionRepository : IRegionRepository
    {
        public Task<Region> CreateRegionAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region> GetRegionAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
