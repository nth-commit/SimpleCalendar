import * as React from 'react';
import { Route, Switch } from 'react-router-dom';
import Layout from './components/containers/Layout';
import AuthCallback from './components/containers/AuthCallback';
import Region from './components/containers/Region';
import { Auth } from 'src/services/Auth';

const NotFound = () => <div>Page not found</div>;

const AuthenticationGuard = ({ children }) => {
  const auth = new Auth();
  if (!auth.isAuthenticated()) {
    auth.login();
    return null;
  }
  return <div>{children}</div>;
};

export const routes = (
  <div>
      <Switch>
        <Route exact={true} path='/callback' component={AuthCallback} />
        <AuthenticationGuard>
          <Layout>
            <Route exact={true} path='/404' component={NotFound} />
            <Route exact={true} path='/' component={Region} />
            <Route path='/:regionId' component={Region} />
          </Layout>
        </AuthenticationGuard>
      </Switch>
  </div>
);
