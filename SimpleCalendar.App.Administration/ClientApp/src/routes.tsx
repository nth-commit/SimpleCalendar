import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import Layout from './components/containers/Layout';
import AuthCallback from './components/containers/AuthCallback';
import Region from './components/containers/Region';
import { Auth } from 'src/services/Auth';
import { appConnect } from 'src/store';
import { rolesActionCreators } from 'src/store/Roles';
import { AuthorizationStatus } from 'src/store/Auth';

const NotFound = () => <div>Page not found</div>;

interface AuthGuardStateProps {
  authorizationStatus: AuthorizationStatus;
}

interface AuthGuardDispatchProps {
  onMount();
}

class UnconnectedAuthGuard extends React.Component<AuthGuardStateProps & AuthGuardDispatchProps> {

  private isAuthenticated: boolean;

  componentWillMount() {
    const auth = new Auth();
    if (!auth.isAuthenticated()) {
      this.isAuthenticated = false;
      auth.login();
    } else {
      this.isAuthenticated = true;
    }
  }

  componentDidMount() {
    if (this.isAuthenticated) {
      this.props.onMount();
    }
  }

  render() {
    if (!this.isAuthenticated) {
      return null;
    }

    if (this.props.authorizationStatus !== AuthorizationStatus.Authorized) {
      return null;
    }

    return <div>{this.props.children}</div>;
  }
}

const AuthGuard = appConnect<AuthGuardStateProps>(
  state => ({
    authorizationStatus: state.auth.status
  }),
  dispatch => ({
    onMount: () => dispatch(rolesActionCreators.fetchRoles())
  })
)(UnconnectedAuthGuard) as any;

export const routes = (
  <Switch>
    <Route exact={true} path='/callback' component={AuthCallback} />
    <AuthGuard>
      <Layout>
        <Route exact={true} path='/404' component={NotFound} />
        <Route exact={true} path='/' component={Region} />
        <Route path='/:regionId' component={Region} />
      </Layout>
    </AuthGuard>
  </Switch>
);
