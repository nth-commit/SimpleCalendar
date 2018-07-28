import { ROOT_REGION_ID } from 'src/constants'
import configureStore from '__testutils__/configureStore'
import { fetchMockRootRegionResponse, fetchMockRegionMembershipsByRegionResponse } from '__testutils__/mocks/fetch'
import { membershipsActionCreators } from '../MembershipsActionCreators'
import regionActionCreators from '../../Regions/ActionCreators'
import { IRegionMembership } from 'src/services/Api'

const createRegionMemberships = (regionId: string, count: number): IRegionMembership[] => {
  return Array.range(count, 1).map(i => {
    return {
      id: i.toString(),
      regionId
    } as IRegionMembership
  })
}

describe('store.memberships.fetchCurrentMemberships', () => {
  const { dispatch, getState } = configureStore()

  it('[SHOULD] fetch all the memberships for the root region [WHEN] the root region is set and fetchCurrentMemberships dispatched', async () => {
    fetchMockRootRegionResponse()
    fetchMockRegionMembershipsByRegionResponse(ROOT_REGION_ID, createRegionMemberships(ROOT_REGION_ID, 5))
    await dispatch(regionActionCreators.setRegion(ROOT_REGION_ID))

    await dispatch(membershipsActionCreators.fetchCurrentMemberships())

    const { memberships } = getState()
    expect(memberships.membershipIdRegionLookup[ROOT_REGION_ID]).toBeDefined()
  })
})
