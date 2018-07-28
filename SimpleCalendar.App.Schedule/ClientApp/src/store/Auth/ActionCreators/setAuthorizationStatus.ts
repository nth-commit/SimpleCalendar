import { ApplicationThunkAction } from '../../'
import { SetAuthorizationStatus } from '../Actions'
import { AuthorizationStatus } from '../State'

export default function setAuthorizationStatus(authorizationStatus: AuthorizationStatus): ApplicationThunkAction {
  return (dispatch) => {
    dispatch({ ...new SetAuthorizationStatus(authorizationStatus) })
  }
}
