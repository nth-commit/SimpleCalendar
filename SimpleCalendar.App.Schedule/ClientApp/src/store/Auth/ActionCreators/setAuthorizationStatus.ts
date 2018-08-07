import { ApplicationThunkAction } from '../../'
import { SetAuthorizationStatus } from '../Actions'
import { AuthenticationStatus } from '../State'

export default function setAuthorizationStatus(authorizationStatus: AuthenticationStatus): ApplicationThunkAction {
  return (dispatch) => {
    dispatch({ ...new SetAuthorizationStatus(authorizationStatus) })
  }
}
