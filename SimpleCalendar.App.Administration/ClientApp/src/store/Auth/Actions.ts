import { Action } from 'redux';
import { IRegionMembership } from 'src/services/Api';

export enum AuthActionTypes {
  FETCH_REGION_MEMBERSHIPS_BEGIN = '[Auth] FETCH_REGION_MEMBERSHIPS_BEGIN',
  FETCH_REGION_MEMBERSHIPS_COMPLETE = '[Auth] FETCH_REGION_MEMBERSHIPS_COMPLETE'
}

export class FetchRegionMembershipsBegin implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_BEGIN;
}

export class FetchRegionMembershipsComplete implements Action {
  readonly type = AuthActionTypes.FETCH_REGION_MEMBERSHIPS_COMPLETE;
  constructor(public regionMemberships: IRegionMembership[]) { }
}

export type AuthAction = 
  FetchRegionMembershipsBegin |
  FetchRegionMembershipsComplete;
