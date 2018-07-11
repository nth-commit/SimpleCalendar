import * as React from 'react';
import { appConnect } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';
import { IRegion } from 'src/services/Api';

interface RegionStateProps {
  loading: boolean;
  regionPath: string;
  childRegions: IRegion[];
}

interface RegionMergedProps {
  loading: boolean;
  onMount(): void;
}

export class UnconnectedRegion extends React.PureComponent<RegionMergedProps> {

  componentDidMount() {
    this.props.onMount();
  }
  
  render() {
    return (
      <div>Hi from Region!</div>
    );
  }
}

export default appConnect<RegionStateProps, {}, RegionMergedProps>(
  (state) => ({
    loading: true,
    regionPath: state.router.location.pathname,
    childRegions: []
  }),
  undefined,
  ({ loading, regionPath, childRegions }, { dispatch }) => ({
    loading,
    childRegions,
    onMount: () => dispatch(regionActionCreators.setRegionByPath(regionPath))
  })
)(UnconnectedRegion);
