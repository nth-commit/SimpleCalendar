import { ROOT_REGION_ID } from 'src/constants'
import configureStore from '__testutils__/configureStore'
import { membershipsActionCreators } from '../MembershipsActionCreators'
import { IRegionMembershipCreate } from 'src/services/Api'
import { fetchMockRegionMembershipCreateResponse, fetchMockRootRegionResponse, fetchMockRegionMembershipsByRegionResponse } from '__testutils__/mocks/fetch'
import { regionActionCreators } from 'src/store/Regions'
import { MembershipDictionaryEntry, MembershipIdRegionLookupEntry } from 'src/store/Memberships/MembershipsState'


describe('store.memberships.createMembership', () => {
  const { dispatch, getState } = configureStore()

  const membershipCreate: IRegionMembershipCreate = {
    regionId: ROOT_REGION_ID,
    regionRoleId: 'ROLE_ADMINISTRATOR',
    userEmail: 'test@example.com'
  }

  it('[SHOULD] create a membership [WHEN] createMembership is dispatched', async () => {
    
    fetchMockRootRegionResponse()
    fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, [])
    fetchMockRegionMembershipCreateResponse(membershipCreate)

    await dispatch(regionActionCreators.setRegion(ROOT_REGION_ID))
    await dispatch(membershipsActionCreators.fetchCurrentMemberships())

    const createMembershipAction = membershipsActionCreators.createMembership(membershipCreate.userEmail, membershipCreate.regionRoleId, membershipCreate.regionId)
    const createMembershipPromise = dispatch(createMembershipAction)

    const membershipsBefore = getState().memberships
    const trackingIdsBeforeComplete = Object.keys(membershipsBefore.membershipCreationDictionary).map(id => parseInt(id, 10))
    expect(trackingIdsBeforeComplete.length).toBe(1)

    const membershipCreationEntry = membershipsBefore.membershipCreationDictionary[trackingIdsBeforeComplete[0]]
    expect((membershipCreationEntry as MembershipDictionaryEntry).membership).toEqual(membershipCreate)

    await createMembershipPromise

    const membershipsAfter = getState().memberships
    expect(Object.keys(membershipsAfter.membershipCreationDictionary).length).toEqual(0)

    const { membershipDictionary, membershipIdRegionLookup } = membershipsAfter
    const membershipRegionEntryInit = membershipIdRegionLookup[ROOT_REGION_ID] as MembershipIdRegionLookupEntry
    const membershipIds = membershipRegionEntryInit.membershipIds as string[]
    expect(membershipIds.length).toEqual(1)

    const membershipDictionaryEntry = membershipDictionary[membershipIds[0]] as MembershipDictionaryEntry
    const { regionId, regionRoleId, userEmail } = membershipDictionaryEntry.membership
    expect({ regionId, regionRoleId, userEmail }).toEqual(membershipCreate)
  })
})
