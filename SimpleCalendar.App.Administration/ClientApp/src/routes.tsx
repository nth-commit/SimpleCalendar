import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/containers/Layout';
import { Home } from './components/containers/Home';
import { AuthCallback } from './components/containers/AuthCallback';

export const routes = (
  <Layout>
    <Route exact={true} path='/' component={Home} />
    <Route exact={true} path='/callback' component={AuthCallback} />
  </Layout>
);
