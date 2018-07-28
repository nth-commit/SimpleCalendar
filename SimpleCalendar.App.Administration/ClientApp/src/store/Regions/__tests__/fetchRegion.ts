import { ROOT_REGION_ID } from 'src/constants'
import { IRegion } from 'src/services/Api'
import configureStore from '__testutils__/configureStore'
import { fetchMockRegionResponse, fetchMock } from '__testutils__/mocks/fetch'
import regionActionCreators from '../ActionCreators'
import { RegionDictionaryEntry } from 'src/store/Regions/RegionsState'

describe('store.regionsNew.fetchRegion', () => {
  const { dispatch, getState } = configureStore()

  it('[SHOULD] fetch the root region [WHEN] fetchRegion is dispatched for root', async () => {
    const matcherName = fetchMockRegionResponse(ROOT_REGION_ID)

    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID))

    expect(fetchMock.calls(matcherName).length).toEqual(1)

    const entry = getState().regions.regionDictionary[ROOT_REGION_ID]
    expect(entry).toBeDefined()

    const entryDefined = entry as RegionDictionaryEntry
    expect(entryDefined.region).not.toBeNull()
    expect((entryDefined.region as IRegion).id).toEqual(ROOT_REGION_ID)
  })

  it('[SHOULD] not fetch the root region twice [WHEN] fetchRegion is dispatched twice for root', async () => {
    const matcherName = fetchMockRegionResponse(ROOT_REGION_ID)

    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID))
    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID))

    expect(fetchMock.calls(matcherName).length).toEqual(1)
  })

  it('[SHOULD] not fetch the root region twice [WHEN] fetchRegion is dispatched twice for root in parallel', async () => {
    const matcherName = fetchMockRegionResponse(ROOT_REGION_ID)

    await Promise.all([
      dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID)),
      dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID))
    ])

    expect(fetchMock.calls(matcherName).length).toEqual(1)
  })

  it('[SHOULD] not fetch root region twice [WHEN] fetchRegion is dispatched twice with no cache', async () => {
    const matcherName = fetchMockRegionResponse(ROOT_REGION_ID)

    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID, -1))
    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID, -1))

    expect(fetchMock.calls(matcherName).length).toEqual(2)
  })
})
