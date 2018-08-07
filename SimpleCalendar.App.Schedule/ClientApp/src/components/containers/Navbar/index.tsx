import * as React from 'react'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import Button from '@material-ui/core/Button'
import { IUser } from 'src/services/Api'
import { appConnect } from 'src/store/ApplicationConnect'
import { authActionCreators } from 'src/store/Auth'
import UserMenuTrigger from '../../presentational/UserMenuTrigger'

export interface NavbarStateProps {
  isAuthenticated: boolean
  profileImageSrc: string | null
}

export interface NavbarDispatchProps {
  login(): void
  logout(): void
}

const Navbar = ({ isAuthenticated, profileImageSrc, login, logout }: NavbarStateProps & NavbarDispatchProps) => {

  return (
    <div style={{ flexGrow: 1 }}>
      <AppBar position="static" color="default">
        <Toolbar style={{ display: 'flex' }}>
          {isAuthenticated ?
            <div style={{ width: '100%', display: 'flex' }}>
              <div style={{ flex: 1 }} />
              <UserMenuTrigger profileImageSrc={profileImageSrc as string} logoutClicked={logout} />
            </div> :
            <div style={{ width: '100%', display: 'flex' }}>
              <div style={{ flex: 1 }} />
              <Button variant="contained" color="primary" onClick={login}>Login</Button>
            </div>
          }
        </Toolbar>
      </AppBar>
    </div>
  )
}

export default appConnect<NavbarStateProps, NavbarDispatchProps>(
  state => {
    const isAuthenticated = !!state.auth.user
    return {
      isAuthenticated,
      profileImageSrc: isAuthenticated ? (state.auth.user as IUser).picture : null
    }
  },
  dispatch => ({
    login: () => dispatch(authActionCreators.login()),
    logout: () => dispatch(authActionCreators.logout())
  })
)(Navbar)
