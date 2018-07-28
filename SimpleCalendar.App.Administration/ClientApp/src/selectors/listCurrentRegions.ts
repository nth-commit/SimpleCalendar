import { IRegion } from 'src/services/Api'
import { ApplicationState } from 'src/store'
import { enumerateRegionId, isBaseRegionId } from 'src/store/Regions'
import getRegionById from './getRegionById'
import getRegionIdOrThrow from 'src/selectors/getRegionIdOrThrow'

export interface ListCurrentRegionOptions {
  ignoreBaseRegions: boolean
}

const DEFAULT_OPTIONS: ListCurrentRegionOptions = {
  ignoreBaseRegions: false
}

export default function listCurrentRegions(state: ApplicationState, opts: ListCurrentRegionOptions = DEFAULT_OPTIONS): IRegion[] {
  return enumerateRegionId(getRegionIdOrThrow(state))
    .filter(r => !opts.ignoreBaseRegions || !isBaseRegionId(r, state.configuration.baseRegionId))
    .map(r => getRegionById(state, r))
}