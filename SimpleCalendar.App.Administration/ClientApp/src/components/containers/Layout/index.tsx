import * as React from 'react';
import { appConnect } from 'src/store';
import { authActionCreators } from 'src/store/Auth';
import { regionActionCreators, areSuperBaseRegionsLoaded } from 'src/store/Regions';
import Navbar from '../Navbar';

export interface LayoutStateProps {
  isLoading: boolean;
}

export interface LayoutDispatchProps {
  onMount(): void;
}

export class UnconnectedLayout extends React.Component<LayoutStateProps & LayoutDispatchProps> {

  componentDidMount() {
    this.props.onMount();
  }

  render() {
    if (this.props.isLoading) {
      return null;
    }

    return (
      <div>
        <Navbar />
        <div>{this.props.children}</div>
      </div>
    );
  }
}

export default appConnect<LayoutStateProps, LayoutDispatchProps>(
  state => ({
    isLoading: !areSuperBaseRegionsLoaded(state)
  }),
  dispatch => ({
    onMount: () => {
      dispatch(authActionCreators.fetchRegionMemberships());
      dispatch(regionActionCreators.fetchBaseRegionParents());
    }
  })
)(UnconnectedLayout) as any;
