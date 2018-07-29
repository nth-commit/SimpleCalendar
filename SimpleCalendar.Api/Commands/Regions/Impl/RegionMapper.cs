using AutoMapper;
using SimpleCalendar.Api.Core.Data;
using SimpleCalendar.Api.Models;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalendar.Api.Commands.Regions.Impl
{
    public class RegionMapper
    {
        private readonly IUserAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IRegionRolesAccessor _regionRolesAccessor;

        public RegionMapper(
            IUserAuthorizationService authorizationService,
            IMapper mapper,
            IRegionRolesAccessor regionRolesAccessor)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _regionRolesAccessor = regionRolesAccessor;
        }

        public Region MapToResult(RegionEntity entity)
        {
            var roles = _regionRolesAccessor.RegionRoles;

            var result = _mapper.Map<Region>(entity);

            result.Permissions = new RegionAuthorization()
            {
                CanAddMemberships = roles.ToDictionary(
                    r => r.Id,
                    r => _authorizationService.CanCreateMembershipAsync(entity, r.Id).Result)
            };

            return result;
        }

        public RegionEntity MapToEntity(RegionCreate create) =>
            _mapper.Map<RegionEntity>(create);
    }
}
