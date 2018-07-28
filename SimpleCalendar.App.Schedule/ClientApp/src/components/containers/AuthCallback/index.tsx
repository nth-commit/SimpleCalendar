import * as React from 'react'
import { Redirect } from 'react-router-dom'
import { Auth } from 'src/services/Auth'

export default class AuthCallback extends React.Component {

  state = {
    authSuccessful: null
  }

  public componentDidMount() {
    new Auth().handleAuthentication()
      .then(() => this.setState({ authSuccessful: true }))
      .catch(() => this.setState({ authSuccessful: false }))
  }

  public render() {
    const { authSuccessful } = this.state
    if (authSuccessful === null) {
      return null
    }
    return <Redirect to={authSuccessful ? '/' : '/error'} />
  }
}
