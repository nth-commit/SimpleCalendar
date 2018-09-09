import * as React from 'react'
import { Route, Switch } from 'react-router-dom'
import Schedule from './components/containers/Schedule'
import AuthCallback from './components/containers/AuthCallback'

export const routes = (
  <Switch>
    <Route path='/callback' exact={true} component={AuthCallback} />
    <Schedule />
  </Switch>
)
