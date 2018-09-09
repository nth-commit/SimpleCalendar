import * as React from 'react'
import { Link } from 'react-router-dom'
import { push } from 'connected-react-router'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import Button from '@material-ui/core/Button'
import Typography from '@material-ui/core/Typography'
import { IUser } from 'src/services/Api'
import { applicationConnect } from 'src/store/ApplicationConnect'
import { authActionCreators } from 'src/store/Auth'
import UserMenuTrigger from '../../presentational/UserMenuTrigger'

export interface NavbarStateProps {
  isAuthenticated: boolean
  showMyEvents: boolean
  profileImageSrc: string | null
}

export interface NavbarDispatchProps {
  login(): void
  logout(): void
  navigateToMyEvents(): void
}

const Navbar = ({
  isAuthenticated,
  profileImageSrc,
  login,
  logout,
  navigateToMyEvents,
  showMyEvents
}: NavbarStateProps & NavbarDispatchProps) => (
  <div style={{ flexGrow: 1 }}>
    <AppBar position="static" color="default">
      <Toolbar style={{ display: 'flex' }}>
        <Link to='/' style={{ textDecoration: 'none', color: 'initial' }}>
          <Typography variant='title' color='inherit'>
            Wellington Vegan Actions
          </Typography>
        </Link>
        <div style={{ flex: 1 }} />
        {isAuthenticated ?
          <div style={{ display: 'flex' }}>
            <div style={{ flex: 1 }} />
            <UserMenuTrigger
              profileImageSrc={profileImageSrc as string}
              myEventsClicked={navigateToMyEvents}
              showMyEvents={showMyEvents}
              logoutClicked={logout} />
          </div> :
          <div style={{ display: 'flex' }}>
            <div style={{ flex: 1 }} />
            <Button variant="contained" color="primary" onClick={login}>Login</Button>
          </div>
        }
      </Toolbar>
    </AppBar>
  </div>
)

export default applicationConnect<NavbarStateProps, NavbarDispatchProps>(
  state => {
    const isAuthenticated = !!state.auth.user
    return {
      isAuthenticated,
      profileImageSrc: isAuthenticated ? (state.auth.user as IUser).picture : null,
      showMyEvents: isAuthenticated ? (state.auth.user as IUser).hasCreatedEvent : false
    }
  },
  dispatch => ({
    login: () => dispatch(authActionCreators.login()),
    logout: () => dispatch(authActionCreators.logout()),
    navigateToMyEvents: () => dispatch(push('/my-events'))
  }),
  undefined,
  {
    pure: false
  }
)(Navbar)
