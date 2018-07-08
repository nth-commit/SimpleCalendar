import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationThunkActionAsync } from '../../';
import { fetchRegion } from './fetchRegion';

export function fetchBaseRegionParents(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { baseRegionId } = getState().configuration;
    if (baseRegionId === ROOT_REGION_ID) {
      return Promise.resolve();
    }

    for (const regionId of getParentRegionIds(baseRegionId)) {
      await dispatch(fetchRegion(regionId));
    }
  };
}

function getParentRegionIds(regionId: string): string[] {
  if (regionId === ROOT_REGION_ID) {
    throw new Error('Region has no parent id');
  }

  let result = [ROOT_REGION_ID];

  const regionIdComponents = regionId.split('/');
  if (regionIdComponents.length > 1) {
    result = [
      ...result,
      ...Array
        .from({ length: regionIdComponents.length - 1 })
        .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'))
    ];
  }

  return result;
}