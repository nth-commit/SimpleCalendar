import * as React from 'react';
import { appConnect } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';

interface RegionStateProps {
  loading: boolean;
}

interface RegionDispatchProps {
  onMounted(): void;
}

export type RegionProps = RegionStateProps & RegionDispatchProps;

export class UnconnectedRegion extends React.PureComponent<RegionProps> {

  componentDidMount() {
    this.props.onMounted();
  }

  render() {
    return (
      <div>Hi from Region!</div>
    );
  }
}

export default appConnect<RegionStateProps, RegionDispatchProps>(
  state => ({
    loading: !state.regions.path.length,
  }),
  dispatch => ({
    onMounted: () => dispatch(regionActionCreators.fetchRootRegion())
  })
)(UnconnectedRegion);
