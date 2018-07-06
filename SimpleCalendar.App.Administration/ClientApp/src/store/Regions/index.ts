import { Reducer } from 'redux';
import { IRegion } from 'components/services/Api';
import { RegionsActionTypes, RegionsAction } from './Actions';
export * from './ActionCreators';

export interface IRegionState {
  path: IRegion[];
}

export const regionsReducer: Reducer = (state: IRegionState, action: RegionsAction): IRegionState => {
  switch (action.type) {

    case RegionsActionTypes.FETCH_REGION_BEGIN:
      return Object.assign({}, state, {});

    case RegionsActionTypes.FETCH_REGION_COMPLETE:
      const regionId = action.region.id;
      const index = regionId === 'ROOT' ? 0 : regionId.split('/').length;
      const level = index + 1;
      const path = state.path;
      return Object.assign({}, state, {
        path: Array
          .from({ length: Math.max(path.length, level )})
          .map((x, i) => index === i ? action.region : path[i])
      })

    default:
      return state || {
        path: []
      };
  }
}
