import { Reducer, DeepPartial } from 'redux'
import { AuthAction, AuthActionTypes } from './Actions'
import { AuthState, AuthenticationStatus } from './State'

const merge = (prevState: AuthState, newStatePartial: DeepPartial<AuthState>): AuthState =>
  Object.assign({}, prevState, newStatePartial)

const DEFAULT_AUTH_STATE: AuthState = {
  status: AuthenticationStatus.Indetermined,
  accessToken: null,
  user: null
}

export const authReducer: Reducer<AuthState, AuthAction> = (state = DEFAULT_AUTH_STATE, action): AuthState => {
  switch (action.type) {
    case AuthActionTypes.LOGIN_SKIPPED: {
      return merge(state, { status: AuthenticationStatus.NotAuthenticated })
    }
    case AuthActionTypes.LOGIN_SUCCESS_BEGIN: {
      return merge(state, { accessToken: action.accessToken })
    }
    case AuthActionTypes.LOGIN_SUCCESS_COMPLETE: {
      return merge(state, { user: action.user, status: AuthenticationStatus.Authenticated })
    }
    case AuthActionTypes.LOGOUT: {
      return merge(state, {
        accessToken: undefined
      })
    }
    case AuthActionTypes.SET_AUTHORIZATION_STATUS: {
      return merge(state, {
        status: action.status
      })
    }
    default:
      return state || {
        status: AuthenticationStatus.Indetermined
      }
  }
}