import { ApplicationThunkActionAsync } from '../../';
import { fetchRegion } from './fetchRegion';

export function fetchRegions(regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    for (const regionIdToFetch of getAllRegionIds(regionId)) {
      await dispatch(fetchRegion(regionIdToFetch));
    }
  };
}

function getAllRegionIds(regionId: string): string[] {
  if (regionId === 'ROOT') {
    return ['ROOT'];
  } else {
    const regionIdComponents = regionId.split('/');
    return [
      'ROOT',
      ...Array
        .from({ length: regionIdComponents.length })
        .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'))
    ];
  }
}