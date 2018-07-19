import { Action } from 'redux';
import { IRegionMembership } from 'src/services/Api';
import { AuthorizationStatus } from './';

export enum AuthActionTypes {
  FETCH_REGION_MEMBERSHIPS_BEGIN = '[Auth] FETCH_REGION_MEMBERSHIPS_BEGIN',
  FETCH_REGION_MEMBERSHIPS_COMPLETE = '[Auth] FETCH_REGION_MEMBERSHIPS_COMPLETE',
  SET_AUTHORIZATION_STATUS = '[Auth] SET_AUTHORIZATION_STATUS'
}

export class FetchRegionMembershipsBegin implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_BEGIN;
}

export class FetchRegionMembershipsComplete implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_COMPLETE;
  constructor(public regionMemberships: IRegionMembership[]) { }
}

export class SetAuthorizationStatus implements Action {
  readonly type = AuthActionTypes.SET_AUTHORIZATION_STATUS;
  constructor(public status: AuthorizationStatus) { }
}

export type AuthAction = 
  FetchRegionMembershipsBegin |
  FetchRegionMembershipsComplete |
  SetAuthorizationStatus;
