import { ApplicationState } from 'src/store'
import { enumerateRegionId } from 'src/store/Regions'

export class RegionNotInitializedError {
  constructor(public regionId: string) { }
}

export default function areRegionsLoading(state: ApplicationState): boolean {
  const { regions } = state

  const { regionId } = regions
  if (!regionId) {
    return true
  }

  const regionIds = enumerateRegionId(regionId)
  return regionIds.some(r => {
    const regionEntry = regions.regionDictionary[r]
    return !regionEntry || !regionEntry.region
  })
}