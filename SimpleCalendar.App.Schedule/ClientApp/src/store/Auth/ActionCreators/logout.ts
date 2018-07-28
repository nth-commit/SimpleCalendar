import { ApplicationThunkAction } from '../../'
import { Logout } from '../Actions'
import { Auth } from 'src/services/Auth'
 
export class AccessTokenNotFoundException { }

export default function setAuthorizationStatus(): ApplicationThunkAction {
  return async (dispatch) => {
    new Auth().logout()
    dispatch({ ...new Logout() })
  }
}
