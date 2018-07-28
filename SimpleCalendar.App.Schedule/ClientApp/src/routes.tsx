import * as React from 'react'
import { Route, Switch } from 'react-router-dom'
import Layout from './components/containers/Layout'
import AuthCallback from './components/containers/AuthCallback'

export const routes = (
  <Switch>
    <Route exact={true} path='/callback' component={AuthCallback} />
    <Layout>
      Hi from app!
    </Layout>
  </Switch>
)
