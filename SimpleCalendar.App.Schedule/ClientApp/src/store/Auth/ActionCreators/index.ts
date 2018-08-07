import loginSkipped from './loginSkipped'
import loginSuccess from './loginSuccess'
import logout from './logout'
import setAuthorizationStatus from './setAuthorizationStatus'
import { Auth } from 'src/services/Auth'
import { ApplicationThunkAction } from 'src/store/ApplicationStore'

export const authActionCreators = {
  login: (): ApplicationThunkAction => () => new Auth().login(),
  loginSkipped,
  loginSuccess,
  logout,
  setAuthorizationStatus
}