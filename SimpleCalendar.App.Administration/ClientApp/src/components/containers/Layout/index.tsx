import * as React from 'react';
import { appConnect } from 'src/store';
import { AuthState, authActionCreators, isAdministrator } from 'src/store/Auth';
import { regionActionCreators, areSuperBaseRegionsLoaded } from 'src/store/Regions';
import Navbar from '../Navbar';

enum AuthorizationStatus {
  Undetermined,
  Successful,
  Unsuccessful
}

export interface LayoutStateProps {
  isLoaded: boolean;
  authorizationStatus: AuthorizationStatus;
}

export interface LayoutDispatchProps {
  onMounted(): void;
}

export type LayoutProps = LayoutStateProps & LayoutDispatchProps;

export class UnconnectedLayout extends React.PureComponent<LayoutProps> {

  componentDidMount() {
    this.props.onMounted();
  }

  render() {
    if (this.props.authorizationStatus !== AuthorizationStatus.Successful) {
      return null;
    }

    if (!this.props.isLoaded) {
      return null;
    }

    return (
      <div>
        <Navbar />
        {this.props.children}
      </div>
    );
  }
}

function getAuthorizationStatus(state: AuthState): AuthorizationStatus {
  if (state.regionMembershipsLoading || !state.regionMemberships) {
    return AuthorizationStatus.Undetermined;
  }
  return isAdministrator(state) ? AuthorizationStatus.Successful : AuthorizationStatus.Unsuccessful;
}

export default appConnect<LayoutStateProps, LayoutDispatchProps>(
  state => ({
    authorizationStatus: getAuthorizationStatus(state.auth),
    isLoaded: areSuperBaseRegionsLoaded(state)
  }),
  dispatch => ({
    onMounted: async () => {
      await dispatch(authActionCreators.fetchRegionMemberships());
      await dispatch(regionActionCreators.fetchBaseRegionParents());
    }
  })
)(UnconnectedLayout) as any;
