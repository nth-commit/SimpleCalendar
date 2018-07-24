import { Action } from 'redux'
import { IRegionMembership, IUser } from 'src/services/Api'
import { AuthorizationStatus } from './'

export enum AuthActionTypes {
  LOGIN_BEGIN = '[Auth] LOGIN_BEGIN',
  LOGIN_COMPLETE = '[Auth] LOGIN_COMPLETE',
  LOGOUT = '[Auth] LOGOUT',
  FETCH_REGION_MEMBERSHIPS_BEGIN = '[Auth] FETCH_REGION_MEMBERSHIPS_BEGIN',
  FETCH_REGION_MEMBERSHIPS_COMPLETE = '[Auth] FETCH_REGION_MEMBERSHIPS_COMPLETE',
  SET_AUTHORIZATION_STATUS = '[Auth] SET_AUTHORIZATION_STATUS'
}

export class LoginBegin implements Action {
  readonly type = AuthActionTypes.LOGIN_BEGIN
  constructor(public accessToken: string) { }
}

export class LoginComplete implements Action {
  readonly type = AuthActionTypes.LOGIN_COMPLETE
  constructor(public user: IUser) { }
}

export class Logout implements Action {
  readonly type = AuthActionTypes.LOGOUT
}

export class FetchRegionMembershipsBegin implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_BEGIN
}

export class FetchRegionMembershipsComplete implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_COMPLETE
  constructor(public regionMemberships: IRegionMembership[]) { }
}

export class SetAuthorizationStatus implements Action {
  readonly type = AuthActionTypes.SET_AUTHORIZATION_STATUS
  constructor(public status: AuthorizationStatus) { }
}

export type AuthAction = 
  LoginBegin |
  LoginComplete |
  Logout |
  FetchRegionMembershipsBegin |
  FetchRegionMembershipsComplete |
  SetAuthorizationStatus
