import { ApplicationThunkActionAsync } from '../../'
import { SetRegion } from '../Actions'
import { enumerateRegionId } from '../Utility'
import { fetchRegion } from './fetchRegion'

export function setRegion(regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {

    dispatch({ ...new SetRegion(regionId) })

    const regionIds = enumerateRegionId(regionId)
    const currentRegionIds = getState().regions.path.map(r => r.id)
    const regionIdsToFetch = regionIds.difference(currentRegionIds)

    await Promise.all(regionIdsToFetch.map(id => dispatch(fetchRegion(id))))
  }
}