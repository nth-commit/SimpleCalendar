import { Reducer, DeepPartial } from 'redux';
import { AuthAction, AuthActionTypes } from './Actions';
import { AuthState, AuthorizationStatus } from './State';

const merge = (prevState: AuthState, newStatePartial: DeepPartial<AuthState>): AuthState =>
  Object.assign({}, prevState, newStatePartial);

export const authReducer: Reducer = (state: AuthState, action: AuthAction): AuthState => {
  switch (action.type) {

    case AuthActionTypes.FETCH_REGION_MEMBERSHIPS_BEGIN:
      return merge(state, {
        regionMembershipsLoading: true
      });
    
    case AuthActionTypes.FETCH_REGION_MEMBERSHIPS_COMPLETE: {
      return merge(state, {
        regionMembershipsLoading: false,
        regionMemberships: action.regionMemberships
      });
    }

    case AuthActionTypes.SET_AUTHORIZATION_STATUS: {
      return merge(state, {
        status: action.status
      });
    }

    default:
      return state || {
        status: AuthorizationStatus.Indetermined,
        accessToken: localStorage.getItem('access_token')
      };
  }
}