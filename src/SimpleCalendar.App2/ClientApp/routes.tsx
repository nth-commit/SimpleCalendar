import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import { AuthLogin, AuthLoginCallback } from './features/Auth';

export const routes = (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/auth/login' component={AuthLogin} />
        <Route exact path='/auth/callback' component={AuthLoginCallback} />
    </Layout>
);
