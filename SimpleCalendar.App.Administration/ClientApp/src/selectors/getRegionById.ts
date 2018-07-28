import { IRegion } from 'src/services/Api'
import { ApplicationState } from 'src/store'
import getRegionEntryById from 'src/selectors/getRegionEntryById'

export class RegionLoadingError {
  constructor(public regionId: string) { }
}

export default function getRegionById(state: ApplicationState, regionId: string): IRegion {
  const entry = getRegionEntryById(state, regionId)

  const { region } = entry
  if (!region) {
    throw new RegionLoadingError(regionId)
  }

  return region
}