import { Reducer, DeepPartial } from 'redux';
import { IRegion, IRegionMembership } from 'src/services/Api';
import { RegionsActionTypes, RegionActions, FetchRegionComplete } from './Actions';
import { enumerateRegionId } from './Utility';
export * from './Utility';
export * from './ActionCreators';

export interface RegionPathComponentValue {
  region: IRegion;
  childRegions: IRegion[];
  memberships: IRegionMembership[];
}

export interface RegionPathComponent {
  id: string;
  loading: boolean;
  value: RegionPathComponentValue | null;
}

export type RegionPath = RegionPathComponent[];

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

const createRegionPathComponentValue = (action: FetchRegionComplete): RegionPathComponentValue => ({
  region: action.region,
  childRegions: action.childRegions,
  memberships: action.memberships
});

const updatePathOnRegionFetch = (path: RegionPath, action: FetchRegionComplete): RegionPath => {
  return path.map(pathComponent => {
    if (pathComponent.id === action.region.id) {
      return mergePathComponent(pathComponent, {
        loading: false,
        value: createRegionPathComponentValue(action)
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
          loading: true,
          value: null
        })
      });

    case RegionsActionTypes.FETCH_REGION_COMPLETE:
      return merge(state, {
        path: updatePathOnRegionFetch(state.path, action)
      });

    default:
      return state || {
        path: []
      };
  }
}