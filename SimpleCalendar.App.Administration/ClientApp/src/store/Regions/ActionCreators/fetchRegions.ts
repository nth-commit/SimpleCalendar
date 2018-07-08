import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationThunkActionAsync } from '../../';
import { fetchRegion } from './fetchRegion';

export class InvalidRegionPath {
  public readonly message: string;
  constructor(regionPath: string) {
    this.message = `${regionPath} is not valid`;
  }
}

export function fetchRegions(regionPath: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    if (!regionPath.startsWith('/')) {
      throw new InvalidRegionPath(regionPath);
    }

    for (const regionIdToFetch of getRegionIds(regionPath, getState().configuration.baseRegionId)) {
      await dispatch(fetchRegion(regionIdToFetch));
    }
  };
}

function getRegionIds(regionPath: string, baseRegionId: string): string[] {
  const requestedRegionId = regionPath.substring(1);
  if (requestedRegionId) {
    return [
      ...enumererateBaseRegionId(baseRegionId),
      ...splitRegionId(requestedRegionId).map(r => getAbsoluteRegionId(r, baseRegionId))
    ];
  } else {
    return enumererateBaseRegionId(baseRegionId);
  }
}

function enumererateBaseRegionId(baseRegionId: string): string[] {
  if (baseRegionId === ROOT_REGION_ID) {
    return [ROOT_REGION_ID];
  } else {
    return [ROOT_REGION_ID, ...splitRegionId(baseRegionId)];
  }
}

function splitRegionId(regionId: string): string[] {
  const regionIdComponents = regionId.split('/');
  return Array
    .from({ length: regionIdComponents.length })
    .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'));
}

function getAbsoluteRegionId(relativeRegionId: string, baseRegionId: string): string {
  if (baseRegionId === ROOT_REGION_ID) {
    return relativeRegionId;
  }
  return `${baseRegionId}/${relativeRegionId}`;
}
