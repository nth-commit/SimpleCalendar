import * as React from 'react'
import { Route, Switch } from 'react-router-dom'
import Layout from './components/containers/Layout'
import AuthCallback from './components/containers/AuthCallback'
import Region from './components/containers/Region'
import AuthGuard from './components/containers/AuthGuard'

const NotFound = () => <div>Page not found</div>

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
)
