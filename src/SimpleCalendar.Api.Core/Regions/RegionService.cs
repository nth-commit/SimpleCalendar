using AutoMapper;
using SimpleCalendar.Api.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionService
    {
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public RegionService(
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        public async Task<RegionEntity> CreateRegionAsync(RegionCreate create)
        {
            RegionEntity parentEntity = null;
            if (string.IsNullOrEmpty(create.ParentId))
            {
                create.ParentId = Data.Constants.RootRegionId;
            }
            else
            {
                parentEntity = await _coreDbContext.Regions.FindAsync(create.ParentId);
                if (parentEntity == null)
                {
                    throw new ArgumentNullException(nameof(RegionCreate.ParentId));
                }
            }

            var entity = _mapper.Map<RegionEntity>(create);
            await _coreDbContext.Regions.AddAsync(entity);
            await _coreDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<RegionEntity> UpdateRegionAsync(string id, RegionUpdate update)
        {
            var entity = await _coreDbContext.Regions.FindAsync(id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }

            var entityUpdated = _mapper.Map(update, entity);
            _coreDbContext.Regions.Update(entityUpdated);
            await _coreDbContext.SaveChangesAsync();

            return entityUpdated;
        }
    }
}
