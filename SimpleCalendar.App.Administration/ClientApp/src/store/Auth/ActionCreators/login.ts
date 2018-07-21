import { ApplicationThunkActionAsync } from '../../';
import { LoginBegin, LoginComplete } from '../Actions';
import { Api } from 'src/services/Api';

export class AccessTokenNotFoundException { }

export default function setAuthorizationStatus(accessToken: string): ApplicationThunkActionAsync {
  return async (dispatch) => {
    dispatch({ ...new LoginBegin(accessToken) });

    const user = await new Api(accessToken).getMyUser();

    dispatch({ ...new LoginComplete(user) });
  };
}
