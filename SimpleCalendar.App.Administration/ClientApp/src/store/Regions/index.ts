import { Reducer, Action } from 'redux';
import { ApplicationThunkAction } from '../';
import { Api, IRegion } from '../../components/services/Api';

export interface IRegionState extends IRegion {
  child?: IRegionState
}

export interface IRegionsState {
  region?: IRegionState;
}

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

export const regionsReducer: Reducer = (state: IRegionsState = {}, action: RegionsAction): IRegionsState => {
  switch (action.type) {

    case RegionsActionTypes.FETCH_REGION_BEGIN:
      return Object.assign({}, state, {});

    case RegionsActionTypes.FETCH_REGION_COMPLETE:
      return Object.assign({}, state, {
        region: {
          id: action.region.id,
          name: action.region.name
        }
      } as IRegionsState)

    default:
      return state;
  }
}

class RegionActionCreators {

  getRegion(regionId: string): ApplicationThunkAction {
    return async (dispatch, getState) => {
      dispatch({ ...new FetchRegionBegin(regionId) });

      const region = await new Api().getRegion(regionId);

      dispatch({ ...new FetchRegionComplete(region) });
    }
  }

}

export const regionActionCreators = new RegionActionCreators();
