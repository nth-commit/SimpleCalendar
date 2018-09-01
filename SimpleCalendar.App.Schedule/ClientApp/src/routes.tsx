import * as React from 'react'
import { Route, Switch } from 'react-router-dom'
import Layout from './components/containers/Layout'
import AuthCallback from './components/containers/AuthCallback'
import EventGroups from './components/containers/EventGroups'

const MyEvents = () => <div>My Events</div>

export const routes = (
  <Switch>
    <Route exact={true} path='/callback' component={AuthCallback} />
    <Layout>
      <Switch>
        <Route exact={true} path='/' component={EventGroups} />
        <Route exact={true} path='/my-events' component={MyEvents} />
      </Switch>
    </Layout>
  </Switch>
)
