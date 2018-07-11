import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationThunkActionAsync } from '../../';
import { setRegion } from './setRegion';

export class InvalidRegionPath {
  public readonly message: string;
  constructor(regionPath: string) {
    this.message = `${regionPath} is not valid`;
  }
}

export function setRegionByPath(regionPath: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    if (!regionPath.startsWith('/')) {
      throw new InvalidRegionPath(regionPath);
    }

    const { baseRegionId } = getState().configuration;

    const regionId = baseRegionId === ROOT_REGION_ID ?
        (regionPath === '/' ? ROOT_REGION_ID : regionPath) :
        baseRegionId + regionPath;
    
    await dispatch(setRegion(removeStartingSlash(removeTrailingSlash(regionId))));
  };
}

function removeTrailingSlash(str: string): string {
  if (str.endsWith('/')) {
    return str.substring(0, str.length - 1);
  }
  return str;
}

function removeStartingSlash(str: string): string {
  if (str.startsWith('/')) {
    return str.substring(1);
  }
  return str;
}