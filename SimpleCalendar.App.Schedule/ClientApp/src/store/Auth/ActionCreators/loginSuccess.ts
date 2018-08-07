import { ApplicationThunkActionAsync } from '../../'
import { LoginSuccessBegin, LoginSuccessComplete } from '../Actions'
import { Api } from 'src/services/Api'

export class AccessTokenNotFoundException { }

export default function loginComplete(accessToken: string): ApplicationThunkActionAsync {
  return async (dispatch) => {
    dispatch({ ...new LoginSuccessBegin(accessToken) })

    const user = await new Api(accessToken).getMyUser()

    dispatch({ ...new LoginSuccessComplete(user) })
  }
}
