﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<RegionResult> GetRegionAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                throw new ArgumentNullException(nameof(publicId));
            }

            var codesJoinedLower = publicId.ToLower();
            var codes = codesJoinedLower.Split('.');

            var region = await _coreDbContext.GetRegionByCodesAsync(codes);
            if (region == null)
            {
                throw new Exception("Entity not found");
            }

            return new RegionResult()
            {
                Id = codesJoinedLower
            };
        }

        public async Task<IEnumerable<RegionResult>> ListRegionsAsync(string parentPublicId)
        {
            var parentCodesJoinedLower = parentPublicId.ToLower();
            var parentCodes = parentCodesJoinedLower.Split('.');

            var region = await _coreDbContext.GetRegionByCodesAsync(parentCodes);
            if (region == null)
            {
                throw new Exception("Entity not found");
            }

            return region.Children.Select(r => new RegionResult
            {
                Id = $"{parentCodesJoinedLower}.{r.Code}"
            });
        }

        public async Task<RegionEntity> CreateRegionAsync(RegionCreate create, string id = null)
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

                // TODO: Ensure max region recursion
            }

            var entity = _mapper.Map<RegionEntity>(create);
            entity.Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;

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