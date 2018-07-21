import { ApplicationThunkAction } from '../../';
import { LoginComplete } from '../Actions';

export class AccessTokenNotFoundException { }

export default function setAuthorizationStatus(accessToken: string): ApplicationThunkAction {
  return (dispatch) => {
    dispatch({ ...new LoginComplete(accessToken) });
  };
}
