import { ROOT_REGION_ID } from 'src/constants'
import configureStore from '__testutils__/configureStore'
import { membershipsActionCreators } from '../MembershipsActionCreators'
import { IRegionMembership } from 'src/services/Api'
import { fetchMockRootRegionResponse, fetchMockRegionMembershipsByRegionResponse, fetchMockRegionMembershipDelete } from '__testutils__/mocks/fetch'
import { regionActionCreators } from 'src/store/Regions'
import { MembershipIdRegionLookupEntry } from 'src/store/Memberships/MembershipsState'

const createRegionMemberships = (regionId: string, count: number): IRegionMembership[] => {
  return Array.range(count, 1).map(i => {
    return {
      id: i.toString(),
      regionId
    } as IRegionMembership
  })
}

describe('store.memberships.deleteMembership', () => {
  const { dispatch, getState } = configureStore()

  it('[SHOULD] delete a membership [WHEN] deleteMembership is dispatched', async () => {
    fetchMockRootRegionResponse()
    const memberships = createRegionMemberships(ROOT_REGION_ID, 1)
    fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, memberships)
    const [membership] = memberships
    fetchMockRegionMembershipDelete(membership.id)

    await dispatch(regionActionCreators.setRegion(ROOT_REGION_ID))
    await dispatch(membershipsActionCreators.fetchCurrentMemberships())

    const membershipsInit = getState().memberships
    const membershipRegionEntryInit = membershipsInit.membershipIdRegionLookup[ROOT_REGION_ID] as MembershipIdRegionLookupEntry
    expect((membershipRegionEntryInit.membershipIds as string[]).length).toEqual(memberships.length)
    expect(Object.keys(membershipsInit.membershipDictionary).length).toEqual(memberships.length)

    const deleteMembershipAction = membershipsActionCreators.deleteMembership(membership.id)
    const deleteMembershipPromise = dispatch(deleteMembershipAction)

    const membershipsBefore = getState().memberships
    const membershipRegionEntryBefore = membershipsBefore.membershipIdRegionLookup[ROOT_REGION_ID] as MembershipIdRegionLookupEntry
    expect((membershipRegionEntryBefore.membershipIds as string[]).length).toEqual(0)
    expect(Object.keys(membershipsBefore.membershipDictionary).length).toEqual(0)

    await deleteMembershipPromise

    // TODO: Keep deleting memberships somewhere
  })
})
