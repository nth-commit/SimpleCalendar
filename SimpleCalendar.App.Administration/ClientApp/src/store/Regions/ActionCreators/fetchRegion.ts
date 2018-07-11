import { ROOT_REGION_ID } from 'src/constants';
import { Api } from 'src/services/Api';
import { RegionState } from 'src/store/Regions';
import { ApplicationThunkActionAsync } from '../../';
import { FetchRegionBegin, FetchRegionComplete } from '../Actions';

export function fetchRegion(regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    validateRegionState(regionId, getState().regions);

    dispatch({ ...new FetchRegionBegin(regionId) });

    const api = new Api();
    const [region, childRegions, regionMemberships] = await Promise.all([
      api.getRegion(regionId),
      api.getRegions(regionId),
      api.getRegionMemberships({ regionId })
    ]);

    dispatch({ ...new FetchRegionComplete(region, childRegions, regionMemberships) });
  };
}

function validateRegionState(regionId: string, regions: RegionState) {
  if (regions.path.length === 0 && regionId !== ROOT_REGION_ID) {
    throw new Error('Root region was not found');
  }

  const regionIdComponents = regionId.split('/');
  const regionLevel = regionIdComponents.length;

  for (let level = 1; level < regionLevel; level++) {
    const expectedAncestorRegionId = regionIdComponents.slice(0, level).join('/');
    if (!regions.path[level]) {
      throw new Error(`Expected ancestor region "${expectedAncestorRegionId}" at path[${level}], but it was not found`);
    }

    const ancestorRegionId = regions.path[level].id;
    if (ancestorRegionId !== expectedAncestorRegionId) {
      throw new Error(`Expected ancestor region "${expectedAncestorRegionId}" at path[${level}], but it was not found`);
    }
  }
}