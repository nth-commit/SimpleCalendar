import * as React from 'react';
import { Route, Switch, withRouter } from 'react-router-dom';
import { Layout } from './components/containers/Layout';
import { Home } from './components/containers/Home';
import { AuthCallback } from './components/containers/AuthCallback';

const _Region = ({ history }) => {
  return (
    <div>
      <h1>Region: {history.location.pathname}</h1>
    </div>
  );
};

const Region = withRouter(_Region);

const NotFound = () => <div>Page not found</div>;

export const routes = (
  <Layout>
    <Switch>
      <Route exact={true} path='/' component={Home} />
      <Route exact={true} path='/callback' component={AuthCallback} />
      <Route exact={true} path='/404' component={NotFound} />
      <Route path='/:regionCode' component={Region} />
    </Switch>
  </Layout>
);
