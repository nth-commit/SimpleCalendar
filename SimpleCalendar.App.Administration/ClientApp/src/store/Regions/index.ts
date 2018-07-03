import { Reducer, Action } from 'redux';
import { ApplicationThunkActionAsync } from '../';
import { Api, IRegion } from '../../components/services/Api';

export interface IRegionState {
  path: IRegion[];
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

export const regionsReducer: Reducer = (state = {} as IRegionState, action: RegionsAction): IRegionState => {
  switch (action.type) {

    case RegionsActionTypes.FETCH_REGION_BEGIN:
      return Object.assign({}, state, {});

    case RegionsActionTypes.FETCH_REGION_COMPLETE:
      return Object.assign({}, state, {
        path: [{
          id: action.region.id,
          name: action.region.name
        }]
      } as IRegionState)

    default:
      return state;
  }
}

export const regionActionCreators = {
  getRegion(regionId: string): ApplicationThunkActionAsync {
    return async (dispatch, getState) => {
      // TODO: throw if root region not requested and this get region is not for the root region.
      
      dispatch({ ...new FetchRegionBegin(regionId) });

      const region = await new Api().getRegion(regionId);

      dispatch({ ...new FetchRegionComplete(region) });
    }
  }
}
