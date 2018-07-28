import { ROOT_REGION_ID } from "src/constants"
import { ApplicationState } from "src/store/ApplicationState"

export function enumerateRegionId(regionId: string): string[] {
  let result = [ROOT_REGION_ID]

  if (regionId !== ROOT_REGION_ID) {
    result = [ROOT_REGION_ID, ...splitRegionId(regionId)]
  }

  return result
}

export function isBaseRegionId(regionId: string, baseRegionId: string): boolean {
  if (regionId === ROOT_REGION_ID) {
    return true
  }

  if (baseRegionId === ROOT_REGION_ID) {
    return false
  }

  return regionId.split('/').length <= baseRegionId.split('/').length
}

export function splitRegionId(regionId: string): string[] {
  const regionIdComponents = regionId.split('/')
  return Array
    .from({ length: regionIdComponents.length })
    .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'))
}

export function getParentRegionIds(regionId: string): string[] {
  if (regionId === ROOT_REGION_ID) {
    throw new Error('Region has no parent id')
  }

  let result = [ROOT_REGION_ID]

  const regionIdComponents = regionId.split('/')
  if (regionIdComponents.length > 1) {
    result = [
      ...result,
      ...Array
        .from({ length: regionIdComponents.length - 1 })
        .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'))
    ]
  }

  return result
}

export function areSuperBaseRegionsLoaded(state: ApplicationState) {
  return true
  // const { baseRegionId } = state.configuration

  // if (baseRegionId === ROOT_REGION_ID) {
  //   return true
  // }

  // const superBaseRegionIds = getParentRegionIds(baseRegionId)
  // return superBaseRegionIds.every(id => {
  //   const pathEntry = state.regions.regions[id]
  //   return !!pathEntry && !pathEntry.
  // })
}