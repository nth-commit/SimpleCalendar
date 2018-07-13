import * as React from 'react';
import { ROOT_REGION_ID } from 'src/constants';
import { appConnect, ApplicationState } from 'src/store';
import { regionActionCreators, RegionPathComponent, RegionPathComponentValue, isPathLoading, pathContainsRegion, getRegionPathComponent } from 'src/store/Regions';
import RegionManagementTabs from '../../presentational/RegionManagementTabs';

interface RegionStateProps {
  loading: boolean;
  regionPathComponent: RegionPathComponent | null;
  regionId: string;
  baseRegionId: string;
}

interface RegionMergedProps {
  loading: boolean;
  regionPathComponent: RegionPathComponent | null;
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

    const { regionPathComponent, baseRegionId } = this.props;
    const { value } = (regionPathComponent as RegionPathComponent);
    const { childRegions } = (value as RegionPathComponentValue);

    return (
      <div>
        <RegionManagementTabs childRegions={childRegions} baseRegionId={baseRegionId} />
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
      regionPathComponent: getRegionPathComponent(state),
      baseRegionId: state.configuration.baseRegionId
    };
  },
  undefined,
  ({ loading, regionPathComponent, regionId, baseRegionId }, { dispatch }) => ({
    loading,
    regionPathComponent,
    baseRegionId,
    onMount: () => dispatch(regionActionCreators.setRegion(regionId))
  })
)(UnconnectedRegion);
