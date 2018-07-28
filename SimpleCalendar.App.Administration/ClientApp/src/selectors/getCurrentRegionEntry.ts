import { ApplicationState } from 'src/store'
import { RegionDictionaryEntry } from 'src/store/Regions'
import getRegionEntryById from 'src/selectors/getRegionEntryById'
import getRegionIdOrThrow from 'src/selectors/getRegionIdOrThrow'

export default function getCurrentRegionEntry(state: ApplicationState): RegionDictionaryEntry {
  return getRegionEntryById(state, getRegionIdOrThrow(state))
}