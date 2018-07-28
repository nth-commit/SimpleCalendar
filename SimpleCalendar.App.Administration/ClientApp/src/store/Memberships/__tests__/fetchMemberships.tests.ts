import { ROOT_REGION_ID } from 'src/constants'
import configureStore from 'test-helpers/configureStore'
import { fetchMockRegionMembershipsByRegionResponse, fetchMock } from 'test-helpers/mocks/fetch'
import { membershipsActionCreators } from '../MembershipsActionCreators'
import { IRegionMembership } from 'src/services/Api'

const createRegionMemberships = (regionId: string, count: number): IRegionMembership[] => {
  return Array.range(count, 1).map(i => {
    return {
      id: i.toString(),
      regionId
    } as IRegionMembership
  })
}

describe('store.memberships.fetchMemberships', () => {
  const { dispatch, getState } = configureStore()

  it('[SHOULD] call have membership entries [WHEN] memberships are fetched for the root region', async () => {
    const regionMemberships = createRegionMemberships(ROOT_REGION_ID, 5)
    const matcherName = fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, regionMemberships)

    await dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID))

    expect(fetchMock.calls(matcherName).length).toEqual(1)

    const { membershipDictionary, membershipIdRegionLookup } = getState().memberships

    const membershipRegionLookupEntry = membershipIdRegionLookup[ROOT_REGION_ID]
    expect(membershipRegionLookupEntry).toBeDefined()
    if (!membershipRegionLookupEntry) { return }

    expect(membershipRegionLookupEntry.regionId).toEqual(ROOT_REGION_ID)

    const { membershipIds } = membershipRegionLookupEntry
    expect(membershipIds).toBeDefined()
    const membershipIdsDefined = membershipIds as string[]
    expect(membershipIdsDefined.length).toEqual(regionMemberships.length)

    membershipIdsDefined.forEach(m => {
      const membership = membershipDictionary[m]
      expect(membership).toBeDefined()
    })
  })

  it('[SHOULD] not fetch the memberships twice [WHEN] fetchMemberships is dispatched twice for root', async () => {
    const matcherName = fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, createRegionMemberships(ROOT_REGION_ID, 5))

    await dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID))
    await dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID))

    expect(fetchMock.calls(matcherName).length).toEqual(1)
  })

  it('[SHOULD] not fetch the memberships twice [WHEN] fetchMemberships is dispatched twice for root in parallel', async () => {
    const matcherName = fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, createRegionMemberships(ROOT_REGION_ID, 5))

    await Promise.all([
      dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID)),
      dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID))
    ])

    expect(fetchMock.calls(matcherName).length).toEqual(1)
  })

  it('[SHOULD] fetch memberships twice [WHEN] fetchMemberships is dispatched twice with no cache', async () => {
    const matcherName = fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, createRegionMemberships(ROOT_REGION_ID, 5))

    await dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID, -1))
    await dispatch(membershipsActionCreators.fetchMemberships(ROOT_REGION_ID, -1))

    expect(fetchMock.calls(matcherName).length).toEqual(2)
  })
})
