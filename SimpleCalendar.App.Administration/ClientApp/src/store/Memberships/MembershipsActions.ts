import { Action } from 'redux'
import { ErrorAction } from 'src/store/ErrorAction'
import { IRegionMembership, IRegionMembershipCreate } from 'src/services/Api'

export enum MembershipActionTypes {
  FETCH_MEMBERSHIPS_BEGIN = '[Memberships] FETCH_MEMBERSHIPS_BEGIN',
  FETCH_MEMBERSHIPS_COMPLETE = '[Memberships] FETCH_MEMBERSHIPS_COMPLETE',
  FETCH_MEMBERSHIPS_ERROR = '[Memberships] FETCH_MEMBERSHIPS_ERROR',
  FETCH_CURRENT_MEMBERSHIPS_BEGIN = '[Memberships] FETCH_CURRENT_MEMBERSHIPS_BEGIN',
  FETCH_CURRENT_MEMBERSHIPS_COMPLETE = '[Memberships] FETCH_CURRENT_MEMBERSHIPS_COMPLETE',
  FETCH_CURRENT_MEMBERSHIPS_ERROR = '[Memberships] FETCH_CURRENT_MEMBERSHIPS_ERROR',
  CREATE_MEMBERSHIP_BEGIN = '[Memberships] CREATE_MEMBERSHIP_BEGIN',
  CREATE_MEMBERSHIP_COMPLETE = '[Memberships] CREATE_MEMBERSHIP_COMPLETE',
  CREATE_MEMBERSHIP_ERROR = '[Memberships] CREATE_MEMBERSHIP_ERROR',
  DELETE_MEMBERSHIP_BEGIN = '[Memberships] DELETE_MEMBERSHIP_BEGIN',
  DELETE_MEMBERSHIP_COMPLETE = '[Memberships] DELETE_MEMBERSHIP_COMPLETE',
  DELETE_MEMBERSHIP_ERROR = '[Memberships] DELETE_MEMBERSHIP_ERROR'
}

export class FetchMembershipsBegin implements Action {
  readonly type = MembershipActionTypes.FETCH_MEMBERSHIPS_BEGIN
  constructor(public regionId: string, public timestamp = new Date().getTime()) { }
}

export class FetchMembershipsComplete implements Action {
  readonly type = MembershipActionTypes.FETCH_MEMBERSHIPS_COMPLETE
  constructor(public regionId: string, public memberships: IRegionMembership[]) { }
}

export class FetchMembershipsError implements ErrorAction {
  readonly type = MembershipActionTypes.FETCH_MEMBERSHIPS_ERROR
  constructor(public error: any) { }
}

export class FetchCurrentMembershipsBegin implements Action {
  readonly type = MembershipActionTypes.FETCH_CURRENT_MEMBERSHIPS_BEGIN
  constructor(public inherit: boolean) { }
}

export class FetchCurrentMembershipsComplete implements Action {
  readonly type = MembershipActionTypes.FETCH_CURRENT_MEMBERSHIPS_COMPLETE
  constructor() { }
}

export class FetchCurrentMembershipsError implements ErrorAction {
  readonly type = MembershipActionTypes.FETCH_CURRENT_MEMBERSHIPS_ERROR
  constructor(public error: any) { }
}

export class CreateMembershipBegin implements Action {
  readonly type = MembershipActionTypes.CREATE_MEMBERSHIP_BEGIN
  constructor(public membership: IRegionMembershipCreate, public trackingId: number, public timestamp: number) { }
}

export class CreateMembershipComplete implements Action {
  readonly type = MembershipActionTypes.CREATE_MEMBERSHIP_COMPLETE
  constructor(public trackingId: number, public membership: IRegionMembership) { }
}

export class CreateMembershipError implements Action {
  readonly type = MembershipActionTypes.CREATE_MEMBERSHIP_ERROR
  constructor(public trackingId: number) { }
}

export class DeleteMembershipBegin implements Action {
  readonly type = MembershipActionTypes.DELETE_MEMBERSHIP_BEGIN
  constructor(public membership: IRegionMembership) { }
}

export class DeleteMembershipComplete implements Action {
  readonly type = MembershipActionTypes.DELETE_MEMBERSHIP_COMPLETE
  constructor(public membershipId: string) { }
}

export class DeleteMembershipError implements ErrorAction {
  readonly type = MembershipActionTypes.DELETE_MEMBERSHIP_ERROR
  constructor(public error: any) { }
}

export type MembershipAction =
  FetchMembershipsBegin |
  FetchMembershipsComplete |
  FetchMembershipsError |
  FetchCurrentMembershipsBegin |
  FetchCurrentMembershipsComplete |
  FetchCurrentMembershipsError |
  CreateMembershipBegin |
  CreateMembershipComplete |
  CreateMembershipError |
  DeleteMembershipBegin |
  DeleteMembershipComplete |
  DeleteMembershipError