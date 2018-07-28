import { Action } from 'redux'
import { IRegion } from 'src/services/Api'

export enum RegionsActionTypes {
  SET_REGION_BEGIN = '[RegionsNew] SET_REGION_BEGIN',
  SET_REGION_COMPLETE = '[RegionsNew] SET_REGION_COMPLETE',
  SET_REGION_ERROR = '[RegionsNew] SET_REGION_ERROR',
  FETCH_REGION_BEGIN = '[RegionsNew] FETCH_REGION_BEGIN',
  FETCH_REGION_COMPLETE = '[RegionsNew] FETCH_REGION_COMPLETE',
  FETCH_REGION_ERROR = '[RegionsNew] FETCH_REGION_ERROR'
}

export class FetchRegionBegin implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_BEGIN
  constructor(public regionId: string, public timestamp = new Date().getTime()) { }
}

export class FetchRegionComplete implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_COMPLETE
  constructor(public region: IRegion, public childRegions: IRegion[]) { }
}

export class FetchRegionError implements Action {
  readonly type = RegionsActionTypes.FETCH_REGION_ERROR
  constructor(public regionId: string, public error?: any) { }
}

export class SetRegionBegin implements Action {
  readonly type = RegionsActionTypes.SET_REGION_BEGIN
  constructor(public regionId: string) { }
}

export class SetRegionComplete implements Action {
  readonly type = RegionsActionTypes.SET_REGION_COMPLETE
  constructor() { }
}

export class SetRegionError implements Action {
  readonly type = RegionsActionTypes.SET_REGION_ERROR
  constructor() { }
}

export type RegionAction = 
  FetchRegionBegin |
  FetchRegionComplete |
  FetchRegionError |
  SetRegionBegin |
  SetRegionComplete |
  SetRegionError
