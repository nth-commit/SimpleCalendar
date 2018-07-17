import * as React from 'react';
import { ROOT_REGION_ID } from 'src/constants';
import { appConnect, ApplicationState } from 'src/store';
import {
  regionActionCreators,
  RegionPath, RegionPathComponentValue,
  isPathLoading, pathContainsRegion
} from 'src/store/Regions';
import RegionManagementTabs from '../../presentational/RegionManagementTabs';
import createRegionHrefResolver from '../../utility/RegionHrefResolver';

interface RegionStateProps {
  loading: boolean;
  regionId: string;
  regionPath: RegionPath;
  baseRegionId: string;
}

interface RegionMergedProps {
  loading: boolean;
  regionId: string;
  regionPath: RegionPath;
  baseRegionId: string;
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

    const { regionPath, regionId, baseRegionId } = this.props;
    const regionIndex = regionPath.findIndex(r => r.id === regionId);

    const regionPathComponentValues = regionPath.map(r => r.value as RegionPathComponentValue);
    const regionPathComponentValue = regionPathComponentValues[regionIndex];
    const parentRegionPathComponentValues = regionPathComponentValues.slice(0, regionIndex);

    return (
      <div>
        <RegionManagementTabs
          childRegions={regionPathComponentValue.childRegions}
          regionHrefResolver={createRegionHrefResolver(baseRegionId)}
          memberships={regionPathComponentValue.memberships}
          inheritedMemberships={parentRegionPathComponentValues.selectMany(r => r.memberships)} />
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
      baseRegionId: state.configuration.baseRegionId
    };
  },
  undefined,
  ({ loading, regionPath, regionId, baseRegionId }, { dispatch }) => ({
    loading,
    regionId,
    regionPath,
    baseRegionId,
    onMount: () => dispatch(regionActionCreators.setRegion(regionId))
  })
)(UnconnectedRegion);
