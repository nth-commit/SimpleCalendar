import { Action } from 'redux';
import { IRegion, IRegionMembership, IRegionMembershipCreate } from 'src/services/Api';

export enum RegionsActionTypes {
  SET_REGION = '[Regions] SET_REGION',
  FETCH_REGION_BEGIN = '[Regions] FETCH_REGION_BEGIN',
  FETCH_REGION_COMPLETE = '[Regions] FETCH_REGION_COMPLETE',
  FETCH_REGION_ERROR = '[Regions] FETCH_REGION_ERROR',
  CREATE_MEMBERSHIP_BEGIN = '[Regions] CREATE_MEMBERSHIP_BEGIN',
  CREATE_MEMBERSHIP_COMPLETE = '[Regions] CREATE_MEMBERSHIP_COMPLETE',
  CREATE_MEMBERSHIP_ERROR = '[Regions] CREATE_MEMBERSHIP_ERROR',
  DELETE_MEMBERSHIP_BEGIN = '[Regions] DELETE_MEMBERSHIP_BEGIN',
  DELETE_MEMBERSHIP_COMPLETE = '[Regions] DELETE_MEMBERSHIP_COMPLETE',
  DELETE_MEMBERSHIP_ERROR = '[Regions] DELETE_MEMBERSHIP_ERROR',
}

export class FetchRegionBegin implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_BEGIN;
  constructor(public regionId: string) { }
}

export class FetchRegionComplete implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_COMPLETE;
  constructor(
    public region: IRegion,
    public childRegions: IRegion[],
    public memberships: IRegionMembership[]) { }
}

export class FetchRegionError implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_ERROR;
  constructor() { }
}

export class SetRegion implements Action {
  readonly type = RegionsActionTypes.SET_REGION;
  constructor(public regionId: string) { }
}

export class CreateMembershipBegin implements Action {
  readonly type = RegionsActionTypes.CREATE_MEMBERSHIP_BEGIN;
  constructor(public membership: IRegionMembershipCreate, public trackingId: number) { }
}

export class CreateMembershipComplete implements Action {
  readonly type = RegionsActionTypes.CREATE_MEMBERSHIP_COMPLETE;
  constructor(public trackingId: number, membership: IRegionMembership) { }
}

export class CreateMembershipError implements Action {
  readonly type = RegionsActionTypes.CREATE_MEMBERSHIP_ERROR;
  constructor(public trackingId: number) { }
}

export class DeleteMembershipBegin implements Action {
  readonly type = RegionsActionTypes.DELETE_MEMBERSHIP_BEGIN;
  constructor(public membershipId: string, public trackingId: number) { }
}

export class DeleteMembershipComplete implements Action {
  readonly type = RegionsActionTypes.DELETE_MEMBERSHIP_COMPLETE;
  constructor(public trackingId: number) { }
}

export class DeleteMembershipError implements Action {
  readonly type = RegionsActionTypes.DELETE_MEMBERSHIP_ERROR;
  constructor(public trackingId: number) { }
}

export type RegionActions = 
  FetchRegionBegin |
  FetchRegionComplete |
  FetchRegionError |
  SetRegion |
  CreateMembershipBegin |
  CreateMembershipComplete |
  CreateMembershipError |
  DeleteMembershipBegin |
  DeleteMembershipComplete |
  DeleteMembershipError;
