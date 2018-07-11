import * as React from 'react';
import { appConnect } from 'src/store';
import { regionActionCreators, RegionPathComponent, isPathLoading, RegionPathComponentValue, getRegionPathComponent } from 'src/store/Regions';

interface RegionStateProps {
  loading: boolean;
  regionPathComponent: RegionPathComponent | null;
  regionPathname: string;
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

export default appConnect<RegionStateProps, {}, {}, RegionMergedProps>(
  (state) => ({
    loading: isPathLoading(state),
    regionPathComponent: getRegionPathComponent(state),
    regionPathname: state.router.location.pathname
  }),
  undefined,
  ({ loading, regionPathComponent, regionPathname }, { dispatch }) => ({
    loading,
    regionPathComponent,
    onMount: () => dispatch(regionActionCreators.setRegionByPath(regionPathname))
  })
)(UnconnectedRegion);
