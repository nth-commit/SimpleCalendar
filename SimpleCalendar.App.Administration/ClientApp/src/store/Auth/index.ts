import { Reducer, DeepPartial } from 'redux';
import { AuthAction, AuthActionTypes } from './Actions';
import { AuthState as _AuthState } from './AuthState';

export type AuthState = _AuthState;
export * from './ActionCreators';
export * from './Utility';

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

    default:
      return state || {};
  }
}
