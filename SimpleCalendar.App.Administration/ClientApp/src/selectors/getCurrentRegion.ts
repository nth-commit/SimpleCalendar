import { IRegion } from 'src/services/Api'
import { ApplicationState } from 'src/store'
import getRegionById from './getRegionById'
import getRegionIdOrThrow from 'src/selectors/getRegionIdOrThrow'

export default function getCurrentRegion(state: ApplicationState): IRegion {
  return getRegionById(state, getRegionIdOrThrow(state))
}