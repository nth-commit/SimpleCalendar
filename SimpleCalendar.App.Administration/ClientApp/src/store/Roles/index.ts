import { Reducer, Action, DeepPartial } from 'redux'
import { ApplicationThunkActionAsync } from '../'
import { Api, IRegionRole } from 'src/services/Api'

export interface RoleState {
  roles: IRegionRole[] | null
  loading: boolean
  error?: any
}

enum RoleActionTypes {
  FETCH_ROLES_BEGIN = '[Regions] FETCH_ROLES_BEGIN',
  FETCH_ROLES_COMPLETE = '[Regions] FETCH_ROLES_COMPLETE',
  FETCH_ROLES_ERROR = '[Regions] FETCH_ROLES_ERROR'
}

class FetchRolesBegin implements Action {
  readonly type = RoleActionTypes.FETCH_ROLES_BEGIN
}

class FetchRolesComplete implements Action {
  readonly type = RoleActionTypes.FETCH_ROLES_COMPLETE
  constructor(public roles: IRegionRole[]) { }
}

class FetchRolesError implements Action {
  readonly type = RoleActionTypes.FETCH_ROLES_ERROR
  constructor(public error: any) { }
}

declare type RolesAction =
  FetchRolesBegin |
  FetchRolesComplete |
  FetchRolesError

const merge = (prevState: RoleState, newStatePartial: DeepPartial<RoleState>): RoleState => {
  return Object.assign({}, prevState, newStatePartial)
}

export const rolesReducer: Reducer = (state: RoleState, action: RolesAction): RoleState => {
    switch (action.type) {
      case RoleActionTypes.FETCH_ROLES_BEGIN:
        return merge(state, { loading: true })
      case RoleActionTypes.FETCH_ROLES_COMPLETE:
        return merge(state, {
          loading: false,
          roles: action.roles
        })
      case RoleActionTypes.FETCH_ROLES_ERROR:
        return merge(state, {
          loading: false,
          error: action.error
        })
      default:
        return state || {}
    }
}

function fetchRoles(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    dispatch(new FetchRolesBegin())

    let roles: IRegionRole[] | null = null
    try {
      roles = await new Api(getState().auth.accessToken).getRegionRoles()
    } catch (e) {
      dispatch(new FetchRolesError(e))
    }

    if (roles !== null) {
      dispatch(new FetchRolesComplete(roles))
    }
  }
}

export const rolesActionCreators = {
  fetchRoles
}
