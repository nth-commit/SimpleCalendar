import * as React from 'react';
import { Auth } from 'src/services/Auth';
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
  isAuthenticationCallback: boolean;
  isLoaded: boolean;
  authorizationStatus: AuthorizationStatus;
}

export interface LayoutDispatchProps {
  onMounted(): void;
}

export type LayoutProps = LayoutStateProps & LayoutDispatchProps;

export class UnconnectedLayout extends React.PureComponent<LayoutProps> {

  private isAuthenticated: boolean;

  componentWillMount() {
    this.isAuthenticated = new Auth().isAuthenticated();
  }

  componentDidMount() {
    if (this.isAuthenticated && !this.props.isAuthenticationCallback) {
      this.props.onMounted();
    }
  }

  render() {
    if (this.props.isAuthenticationCallback) {
      return <div>{this.props.children}</div>
    }

    if (!this.isAuthenticated) {
      new Auth().login();
      return null;
    }

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
    isAuthenticationCallback: state.router.location.pathname === '/callback',
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
