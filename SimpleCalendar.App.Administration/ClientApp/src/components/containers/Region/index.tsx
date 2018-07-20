import * as React from 'react';
import { ROOT_REGION_ID } from 'src/constants';
import { appConnect, ApplicationState } from 'src/store';
import {
  regionActionCreators,
  RegionPath, RegionPathComponentValue,
  isPathLoading, pathContainsRegion
} from 'src/store/Regions';
import { IRegionRole } from 'src/services/Api';
import RegionManagementTabs from '../../presentational/RegionManagementTabs';
import createRegionHrefResolver from '../../utility/RegionHrefResolver';

interface RegionStateProps {
  loading: boolean;
  regionId: string;
  regionPath: RegionPath;
  baseRegionId: string;
  roles: IRegionRole[];
}

interface RegionMergedProps {
  loading: boolean;
  regionId: string;
  regionPath: RegionPath;
  baseRegionId: string;
  roles: IRegionRole[];
  onMount(): void;
}

export class UnconnectedRegion extends React.PureComponent<RegionMergedProps> {

  componentDidMount() {
    this.props.onMount();
  }
  
  render() {
    if (this.props.loading) {
      return null;
    }

    const { regionPath, regionId, baseRegionId, roles } = this.props;
    const regionIndex = regionPath.findIndex(r => r.id === regionId);

    const regionPathComponentValues = regionPath.map(r => r.value as RegionPathComponentValue);
    const regionPathComponentValue = regionPathComponentValues[regionIndex];
    const parentRegionPathComponentValues = regionPathComponentValues.slice(0, regionIndex);

    const region = regionPathComponentValue.region;

    const memberships = regionPathComponentValue.memberships;
    const inheritedMemberships = parentRegionPathComponentValues.selectMany(r => r.memberships);
    const membershipRoleIds = Set.fromArray([...memberships, ...inheritedMemberships], m => m.regionRoleId);

    return (
      <div>
        <RegionManagementTabs
          roles={roles.filter(r => region.permissions.canAddMemberships[r.id] || membershipRoleIds.has(r.id))}
          childRegions={regionPathComponentValue.childRegions}
          regionHrefResolver={createRegionHrefResolver(baseRegionId)}
          memberships={memberships}
          inheritedMemberships={inheritedMemberships} />
      </div>
    );
  }
}

const stripTrailingSlash = (str: string) => str.endsWith('/') ? str.substring(0, str.length - 1) : str;
const stripLeadingSlash = (str: string) => str.startsWith('/') ? str.substring(1) : str;

const getRegionId = (state: ApplicationState): string => {
  const { baseRegionId } = state.configuration;
  const { pathname } = state.router.location;

  const regionId = baseRegionId === ROOT_REGION_ID ?
    (pathname === '/' ? ROOT_REGION_ID : pathname) :
    baseRegionId + pathname;
  
  return stripLeadingSlash(stripTrailingSlash(regionId));
}

export default appConnect<RegionStateProps, {}, {}, RegionMergedProps>(
  (state) => {
    const regionId = getRegionId(state);
    return {
      regionId,
      loading: isPathLoading(state) || !pathContainsRegion(state, regionId),
      regionPath: state.regions.path,
      baseRegionId: state.configuration.baseRegionId,
      roles: state.roles.roles
    };
  },
  undefined,
  (stateProps, { dispatch }) => ({
    ...stateProps,
    onMount: () => dispatch(regionActionCreators.setRegion(stateProps.regionId))
  })
)(UnconnectedRegion);
