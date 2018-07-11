import { ROOT_REGION_ID } from 'src/constants';

export function getRegionIds(regionPath: string, baseRegionId: string): string[] {
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

export function enumerateRegionId(regionId: string): string[] {
  let result = [ROOT_REGION_ID];

  if (regionId !== ROOT_REGION_ID) {
    result = [ROOT_REGION_ID, ...splitRegionId(regionId)];
  }

  return result;
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
