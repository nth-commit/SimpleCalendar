import * as React from 'react';
import { appConnect } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';

interface RegionStateProps {
  loading: boolean;
  regionId: string;
}

interface RegionDispatchProps {
  onMount(): void;
}

interface RegionMergedProps {
  loading: boolean;
  onMount(): void;
}

export type RegionProps = RegionStateProps & RegionDispatchProps;

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
    regionId: state.router.location.pathname
  }),
  undefined,
  ({ loading, regionId }, { dispatch }) => ({
    loading,
    onMount: () => dispatch(regionActionCreators.fetchRegions(regionId))
  })
)(UnconnectedRegion);
