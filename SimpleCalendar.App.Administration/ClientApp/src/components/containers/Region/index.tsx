import * as React from 'react';
import { ROOT_REGION_ID } from 'src/constants';
import { appConnect, ApplicationState } from 'src/store';
import { regionActionCreators, RegionPathComponent, isPathLoading, pathContainsRegion, RegionPathComponentValue, getRegionPathComponent } from 'src/store/Regions';

interface RegionStateProps {
  loading: boolean;
  regionPathComponent: RegionPathComponent | null;
  regionId: string;
}

interface RegionMergedProps {
  loading: boolean;
  regionPathComponent: RegionPathComponent | null;
  onMount(): void;
}

export class UnconnectedRegion extends React.PureComponent<RegionMergedProps> {

  componentDidMount() {
    this.props.onMount();
  }
  
  render() {
    if (this.props.loading) {
      return <div>LOADING!</div>;
    }

    const { regionPathComponent } = this.props;
    const { value } = (regionPathComponent as RegionPathComponent);
    const { region } = (value as RegionPathComponentValue);

    return (
      <div>Hi from {region.name}!</div>
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
      loading: isPathLoading(state) || !pathContainsRegion(state, regionId),
      regionPathComponent: getRegionPathComponent(state),
      regionId
    };
  },
  undefined,
  ({ loading, regionPathComponent, regionId }, { dispatch }) => ({
    loading,
    regionPathComponent,
    onMount: () => dispatch(regionActionCreators.setRegion(regionId))
  })
)(UnconnectedRegion);
