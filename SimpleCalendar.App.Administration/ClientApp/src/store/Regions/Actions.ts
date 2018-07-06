import { Action } from 'redux';
import { IRegion } from 'components/services/Api';

export enum RegionsActionTypes {
  FETCH_REGION_BEGIN = '[Regions] FETCH_REGION_BEGIN',
  FETCH_REGION_COMPLETE = '[Regions] FETCH_REGION_COMPLETE',
  FETCH_REGION_ERROR = '[Regions] FETCH_REGION_ERROR'
}

export class FetchRegionBegin implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_BEGIN;
  constructor(public regionId: string) { }
}

export class FetchRegionComplete implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_COMPLETE;
  constructor(public region: IRegion) { }
}

export class FetchRegionError implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_ERROR;
  constructor() { }
}

export type RegionsAction = 
  FetchRegionBegin |
  FetchRegionComplete |
  FetchRegionError;
