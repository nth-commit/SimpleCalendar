import { ApplicationThunkActionAsync } from 'src/store/ApplicationStore'
import { enumerateRegionId } from 'src/store/Regions/Utility'
import { FetchCurrentMembershipsBegin, FetchCurrentMembershipsComplete, FetchCurrentMembershipsError } from 'src/store/Memberships/MembershipsActions'
import { fetchMemberships } from 'src/store/Memberships/MembershipsActionCreators/fetchMemberships'

export class NoCurrentRegionFoundError { }

export function fetchCurrentMemberships(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { regions } = getState()
    const { regionId } = regions

    if (!regionId) {
      throw new NoCurrentRegionFoundError()
    }

    dispatch(new FetchCurrentMembershipsBegin(false))

    const regionIdsToFetch = enumerateRegionId(regionId)
    try {
      await Promise.all(regionIdsToFetch.map(regionIdToFetch => dispatch(fetchMemberships(regionIdToFetch))))
      dispatch(new FetchCurrentMembershipsComplete())
    } catch (e) {
      dispatch(new FetchCurrentMembershipsError(e))
    }
  }
}