import * as React from 'react';
import { Route, Switch, withRouter } from 'react-router-dom';
import Layout from './components/containers/Layout';
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

// TODO: Better typings for this?
const LayoutAny = Layout as any;

export const routes = (
  <div>
    <LayoutAny>
      <Switch>
        <Route exact={true} path='/callback' component={AuthCallback} />
        <Route exact={true} path='/404' component={NotFound} />
        <Route path='/:regionId' component={Region} />
      </Switch>
    </LayoutAny>
  </div>
);
