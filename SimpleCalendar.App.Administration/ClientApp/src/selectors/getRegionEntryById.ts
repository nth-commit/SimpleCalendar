import { ApplicationState } from 'src/store'
import { RegionNotInitializedError } from 'src/selectors/areRegionsLoading'
import { RegionDictionaryEntry } from 'src/store/Regions'

export default function getRegionEntryById(state: ApplicationState, regionId: string): RegionDictionaryEntry {
  const { regions } = state

  const entry = regions.regionDictionary[regionId]
  if (!entry) {
    throw new RegionNotInitializedError(regionId)
  }

  return entry
}