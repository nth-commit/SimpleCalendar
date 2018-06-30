import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/containers/Layout';
import { Home } from './components/containers/Home';

export const routes = (
  <Layout>
    <Route exact={true} path='/' component={Home} />
    {/* <Route exact={true} path='/auth/login' component={AuthLogin} />
    <Route exact={true} path='/auth/callback' component={AuthLoginCallback} /> */}
  </Layout>
);
