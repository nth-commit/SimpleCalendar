import { ApplicationThunkActionAsync } from 'src/store'
import { SetRegionBegin, SetRegionComplete } from '../Actions'
import { enumerateRegionId } from '../Utility'
import { fetchRegion } from './fetchRegion'

export function setRegion(regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {

    dispatch({ ...new SetRegionBegin(regionId) })

    const regionIds = enumerateRegionId(regionId)
    const currentRegionIds = Object.keys(getState().regions.regionDictionary)
    const regionIdsToFetch = regionIds.difference(currentRegionIds)

    const fetchRegionPromises = regionIdsToFetch.map(id => dispatch(fetchRegion(id)))
    await Promise.all(fetchRegionPromises)

    dispatch({ ...new SetRegionComplete() })
  }
}