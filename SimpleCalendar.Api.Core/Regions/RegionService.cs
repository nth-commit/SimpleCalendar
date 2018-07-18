using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Utility.Authorization;
using SimpleCalendar.Utiltiy.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Core.Regions
{
    public class RegionService
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly CoreDbContext _coreDbContext;
        private readonly IMapper _mapper;

        public RegionService(
            IUserAuthorizationService authorizationService,
            CoreDbContext coreDbContext,
            IMapper mapper)
        {
            _authorizationService = authorizationService;
            _coreDbContext = coreDbContext;
            _mapper = mapper;
        }

        public async Task<RegionResult> GetRegionAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var region = await _coreDbContext.GetRegionByCodesAsync(id);
            if (region == null)
            {
                return null;
            }

            var roles = await _coreDbContext.RegionRoles.ToListAsync();
            var result = _mapper.Map<RegionResult>(region);
            result.Permissions = new RegionAuthorization()
            {
                CanAddMemberships = roles.ToDictionary(
                    r => r.Id,
                    r => _authorizationService.CanCreateMembershipAsync(region, r.Id).Result)
            };

            return result;
        }

        public async Task<IEnumerable<RegionResult>> ListRegionsAsync(string parentId)
        {
            var region = await _coreDbContext.GetRegionByCodesAsync(parentId ?? string.Empty);
            if (region == null)
            {
                Validator.ThrowInvalid(nameof(parentId), $"Could not find region with {nameof(parentId)} \"{parentId}\"");
            }

            var roles = await _coreDbContext.RegionRoles.ToListAsync();
            return region.Children.Select(r =>
            {
                var result = _mapper.Map<RegionResult>(r);
                result.Permissions = new RegionAuthorization()
                {
                    CanAddMemberships = roles.ToDictionary(
                        rr => rr.Id,
                        rr => _authorizationService.CanCreateMembershipAsync(region, rr.Id).Result)
                };
                return result;
            });
        }

        public async Task<RegionCreateResult> CreateRegionAsync(RegionCreate create)
        {
            RegionEntity parentEntity = null;
            if (!string.IsNullOrEmpty(create.ParentId))
            {
                parentEntity = await _coreDbContext.GetRegionByCodesAsync(create.ParentId);
                if (parentEntity == null)
                {
                    return RegionCreateResult.ParentRegionNotFound;
                }

                var parentLevel = parentEntity.GetLevel();
                if (parentLevel >= 5)
                {
                    return RegionCreateResult.MaxRegionLevelReached;
                }
            }

            var entity = _mapper.Map<RegionEntity>(create);
            if (parentEntity == null)
            {
                entity.ParentId = Data.Constants.RootRegionId;
            }
            else
            {
                entity.Parent = parentEntity;
            }

            var existingEntity = await _coreDbContext.GetRegionByCodesAsync(entity.GetId());
            if (existingEntity != null)
            {
                return RegionCreateResult.RegionAlreadyExists;
            }

            await _coreDbContext.Regions.AddAsync(entity);
            await _coreDbContext.SaveChangesAsync();

            return RegionCreateResult.Success(entity);
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
