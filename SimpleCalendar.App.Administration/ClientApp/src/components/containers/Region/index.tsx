import * as React from 'react';
// import { connect } from 'react-redux';
// import { IApplicationState } from 'src/store';
// import { regionActionCreators } from 'src/store/Regions';

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

export default UnconnectedRegion;

// export default connect(
//   state => ({ state }),
//   dispatch => ({ dispatch }),
//   (stateProps, dispatchProps) => {
//     const { state } = stateProps;
//     const { dispatch } = dispatchProps;

//     return {
//       loading: false
//       // onMounted: () => dispatch(regionActionCreators.fetchRegion(''))
//     } as RegionProps;
//   }

//   // dispatch => ({
//   //   onMounted: () => {}
//   // })
// )(UnconnectedRegion);
