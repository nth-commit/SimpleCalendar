import { ApplicationThunkActionAsync } from '../../';
import { FetchRegionBegin, FetchRegionComplete } from '../Actions';
import { Api } from 'components/services/Api';
import { IRegionState } from 'store/Regions';

export function fetchRegion(regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    validateRegionState(regionId, getState().regions);

    dispatch({ ...new FetchRegionBegin(regionId) });

    const region = await new Api().getRegion(regionId);

    dispatch({ ...new FetchRegionComplete(region) });
  };
}

function validateRegionState(regionId: string, regions: IRegionState) {
  if (regions.path.length === 0 && regionId !== 'ROOT') {
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