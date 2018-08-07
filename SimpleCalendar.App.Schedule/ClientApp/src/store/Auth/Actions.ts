import { Action } from 'redux'
import { IRegionMembership, IUser } from 'src/services/Api'
import { AuthenticationStatus } from './'

export enum AuthActionTypes {
  LOGIN_SKIPPED = '[Auth] LOGIN_SKIPPED',
  LOGIN_SUCCESS_BEGIN = '[Auth] LOGIN_SUCCESS_BEGIN',
  LOGIN_SUCCESS_COMPLETE = '[Auth] LOGIN_SUCCESS_COMPLETE',
  LOGOUT = '[Auth] LOGOUT',
  FETCH_REGION_MEMBERSHIPS_BEGIN = '[Auth] FETCH_REGION_MEMBERSHIPS_BEGIN',
  FETCH_REGION_MEMBERSHIPS_COMPLETE = '[Auth] FETCH_REGION_MEMBERSHIPS_COMPLETE',
  SET_AUTHORIZATION_STATUS = '[Auth] SET_AUTHORIZATION_STATUS'
}

export class LoginSkipped implements Action {
  readonly type = AuthActionTypes.LOGIN_SKIPPED
}

export class LoginSuccessBegin implements Action {
  readonly type = AuthActionTypes.LOGIN_SUCCESS_BEGIN
  constructor(public accessToken: string) { }
}

export class LoginSuccessComplete implements Action {
  readonly type = AuthActionTypes.LOGIN_SUCCESS_COMPLETE
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
  constructor(public status: AuthenticationStatus) { }
}

export type AuthAction = 
  LoginSkipped |
  LoginSuccessBegin |
  LoginSuccessComplete |
  Logout |
  FetchRegionMembershipsBegin |
  FetchRegionMembershipsComplete |
  SetAuthorizationStatus
