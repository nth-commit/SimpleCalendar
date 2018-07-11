import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationState } from 'src/store/ApplicationState';
import { RegionState, RegionPath, RegionPathComponent } from 'src/store/Regions';

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

export function getRegionPathAboveBase(state: ApplicationState): RegionPath {
  const { baseRegionId } = state.configuration;
  const { path } = state.regions;
  const result = path.filter(r => r.id !== 'ROOT' || r.id.split('/').length > baseRegionId.split('/').length);
  return result;
}

function isState(state: ApplicationState | RegionState | RegionPath): state is ApplicationState {
  return 'regions' in state;
}

function isRegionsState(state: RegionState | RegionPath): state is RegionState {
  return 'path' in state;
}

export function isPathLoading(state: ApplicationState | RegionState | RegionPath): boolean {
  if (isState(state)) {
    return isPathLoading(state.regions);
  }

  if (isRegionsState(state)) {
    return isPathLoading(state.path);
  }
  
  return state.length === 0 || state.some(r => r.loading);
}

export function getRegionPathComponent(state: ApplicationState): RegionPathComponent | null {
  return state.regions.path.find(p => p.id === state.regions.regionId) || null;
}

export function getParentRegionIds(regionId: string): string[] {
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

export function areSuperBaseRegionsLoaded(state: ApplicationState) {
  const { baseRegionId } = state.configuration;

  if (baseRegionId === ROOT_REGION_ID) {
    return true;
  }

  const superBaseRegionIds = getParentRegionIds(baseRegionId);
  return superBaseRegionIds.every(id => {
    const pathEntry = state.regions.path.find(p => p.id === id);
    return !!pathEntry && !pathEntry.loading;
  });
}