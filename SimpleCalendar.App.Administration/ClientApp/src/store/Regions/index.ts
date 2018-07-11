import { Reducer, DeepPartial } from 'redux';
import { IRegion } from 'src/services/Api';
import { RegionsActionTypes, RegionActions } from './Actions';
import { enumerateRegionId } from './Utility';
export * from './ActionCreators';

export interface RegionPathComponentValue {
  region: IRegion;
}

export interface RegionPathComponent {
  id: string;
  value: RegionPathComponentValue | null;
}

declare type RegionPath = RegionPathComponent[];

export interface RegionState {
  regionId: string;
  path: RegionPath;
}

const merge = (prevState: RegionState, newStatePartial: DeepPartial<RegionState>): RegionState =>
  Object.assign({}, prevState, newStatePartial);

const mergePathComponent = (prevPathComponent: RegionPathComponent, newPathComponentPartial: DeepPartial<RegionPathComponent>): RegionPathComponent =>
  Object.assign({}, prevPathComponent, newPathComponentPartial);


const getSubPath = (path: RegionPath, regionId: string): RegionPath => {
  if (path.length === 0) {
    return [];
  }

  const prevRegionIds = path.map(r => r.id);
  const newRegionIds = enumerateRegionId(regionId);
  const firstNonMatchingIndex = prevRegionIds.findIndex((id, index) => id !== newRegionIds[index]);

  const subPathIsCurrentPath = firstNonMatchingIndex < 0;
  return subPathIsCurrentPath ? path : path.slice(0, firstNonMatchingIndex);
}

const mergePath = (path: RegionPath, pathComponent: RegionPathComponent): RegionPath => {
  return [
    ...getSubPath(path, pathComponent.id),
    pathComponent
  ];
}

const updatePath = (path: RegionPath, region: IRegion): RegionPath => {
  return path.map(pathComponent => {
    if (pathComponent.id === region.id) {
      return mergePathComponent(pathComponent, {
        value: {
          region
        }
      });
    }
    return pathComponent;
  })
}

export const regionsReducer: Reducer = (state: RegionState, action: RegionActions): RegionState => {
  switch (action.type) {

    case RegionsActionTypes.SET_REGION:
      return merge(state, {
        regionId: action.regionId
      });
    
    case RegionsActionTypes.FETCH_REGION_BEGIN:
      return merge(state, {
        path: mergePath(state.path, {
          id: action.regionId,
          value: null
        })
      });

    case RegionsActionTypes.FETCH_REGION_COMPLETE:
      return merge(state, {
        path: updatePath(state.path, action.region)
      });

    default:
      return state || {
        path: []
      };
  }
}