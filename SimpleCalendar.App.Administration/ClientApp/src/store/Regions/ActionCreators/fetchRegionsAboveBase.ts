import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationThunkActionAsync } from '../../';
import { fetchRegions } from './fetchRegions';

export function fetchRegionsAboveBase(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { baseRegionId } = getState().configuration;
    if (baseRegionId === ROOT_REGION_ID) {
      return Promise.resolve();
    }

    await dispatch(fetchRegions(getParentRegionId(baseRegionId)));
  };
}

function getParentRegionId(regionId: string): string {
  if (regionId === ROOT_REGION_ID) {
    throw new Error('Region has not parent id');
  }

  const regionIdComponents = regionId.split('/');
  return regionIdComponents.length === 1 ?
    ROOT_REGION_ID :
    regionIdComponents.slice(0, regionIdComponents.length - 1).join('/');
}
