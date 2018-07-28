import { ApplicationThunkActionAsync } from 'src/store/ApplicationStore'
import { Api } from 'src/services/Api'
import { FetchMembershipsBegin, FetchMembershipsComplete, FetchMembershipsError } from 'src/store/Memberships/MembershipsActions'
import { MembershipIdRegionLookupEntry } from 'src/store/Memberships/MembershipsState'

const outstandingRegionMembershipRequests = new Map<string, Promise<void>>()

const isMembershipRegionLookupEntryExpired = (membershipRegionLookupEntry: MembershipIdRegionLookupEntry, maxAge: number) =>
  new Date().getTime() - (membershipRegionLookupEntry.timestamp as number) > maxAge

export function fetchMemberships(regionId: string, maxAge: number = 2 * 60 * 1000): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { auth, memberships } = getState()

    const existingEntry = memberships.membershipIdRegionLookup[regionId]
    if (existingEntry && !isMembershipRegionLookupEntryExpired(existingEntry, maxAge)) {
      const outstandingRequest = outstandingRegionMembershipRequests.get(regionId)
      if (outstandingRequest) {
        await outstandingRequest
      }
      return
    }

    dispatch(new FetchMembershipsBegin(regionId))

    const api = new Api(auth.accessToken)
    const regionMembershipsPromise = api.getRegionMemberships({ regionId })

    try {
      const regionMemberships = await regionMembershipsPromise
      dispatch(new FetchMembershipsComplete(regionId, regionMemberships))
    } catch (e) {
      dispatch(new FetchMembershipsError(e))
    } finally {
      outstandingRegionMembershipRequests.delete(regionId)
    }
  }
}