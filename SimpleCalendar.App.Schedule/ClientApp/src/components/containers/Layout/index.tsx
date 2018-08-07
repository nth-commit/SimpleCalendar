import * as React from 'react'
import DialogTrigger from '../DialogTrigger'
import Navbar from '../Navbar'
import Effects from '../Effects'
import { appConnect } from 'src/store'
import { Auth } from 'src/services/Auth'
import { authActionCreators, AuthenticationStatus } from 'src/store/Auth'

interface LayoutStateProps {
  isLoading: boolean
}

interface LayoutDispatchProps {
  didMount(): void
}

class Layout extends React.PureComponent<LayoutStateProps & LayoutDispatchProps> {

  componentDidMount() { this.props.didMount() }

  render() {
    const { isLoading, children } = this.props
    if (isLoading) {
      return null
    }

    return (
      <div>
        <Navbar />
        {children}
        <DialogTrigger />
        <Effects />
      </div>
    )
  }
}

export default appConnect<LayoutStateProps, LayoutDispatchProps>(
  state => ({
    isLoading: state.auth.status === AuthenticationStatus.Indetermined
  }),
  dispatch => ({
    didMount: () => {
      if (new Auth().isAuthenticated()) {
        dispatch(authActionCreators.loginSuccess(localStorage.getItem('access_token') as string))
      } else {
        dispatch(authActionCreators.loginSkipped())
      }
    }
  })
)(Layout)