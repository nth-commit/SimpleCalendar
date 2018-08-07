import * as React from 'react'
import { Auth } from 'src/services/Auth'
import { appConnect } from 'src/store'
import { rolesActionCreators } from 'src/store/Roles'
import { authActionCreators, AuthorizationStatus } from 'src/store/Auth'

interface AuthGuardStateProps {
  authorizationStatus: AuthorizationStatus
}

interface AuthGuardDispatchProps {
  onMount()
}

class UnconnectedAuthGuard extends React.Component<AuthGuardStateProps & AuthGuardDispatchProps> {

  private isAuthenticated: boolean

  componentWillMount() {
    const auth = new Auth()
    if (!auth.isAuthenticated()) {
      this.isAuthenticated = false
      auth.login()
    } else {
      this.isAuthenticated = true
    }
  }

  componentDidMount() {
    if (this.isAuthenticated) {
      this.props.onMount()
    }
  }

  render() {
    if (!this.isAuthenticated) {
      return null
    }

    if (this.props.authorizationStatus !== AuthorizationStatus.Authorized) {
      return null
    }

    return <div>{this.props.children}</div>
  }
}

export default appConnect<AuthGuardStateProps>(
  state => ({
    authorizationStatus: state.auth.status
  }),
  dispatch => ({
    onMount: async () => {
      await dispatch(authActionCreators.login(localStorage.getItem('access_token') as string))
      dispatch(rolesActionCreators.fetchRoles())
    }
  })
)(UnconnectedAuthGuard) as any