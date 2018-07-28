import { ROOT_REGION_ID } from 'src/constants'
import configureStore from 'test-helpers/configureStore'
import { fetchMockRootRegionResponse, fetchMockRegionResponse, createRegionResponse } from 'test-helpers/mocks/fetch'
import regionActionCreators from '../ActionCreators'
import { enumerateRegionId } from 'src/store/Regions/Utility'
import { IRegion } from 'src/services/Api'
import { RegionDictionaryEntry } from '../RegionsState'

describe('store.regionsNew.setRegion', () => {
  const NEW_ZEALAND_REGION_ID = 'new-zealand'
  const WELLINGTON_REGION_ID = 'new-zealand/wellington'
  const AUCKLAND_REGION_ID = 'new-zealand/auckland'
  const AUSTRALIA_REGION_ID = 'australia'
  const { dispatch, getState } = configureStore()

  const dispatchSetRegion = async (regionId: string) => {
    const setRegionPromise = dispatch(regionActionCreators.setRegion(regionId))
    expect(getState().regions.regionId).toBe(regionId)
    await setRegionPromise
  }

  const expectRegion = (expectedRegionId: string) => {

    const { regionId, regionDictionary } = getState().regions

    expect(regionId).toBe(expectedRegionId)

    enumerateRegionId(expectedRegionId).forEach(r => {
      const regionEntry = regionDictionary[r]
      expect(regionEntry).toBeDefined()

      const region = (regionEntry as RegionDictionaryEntry).region as IRegion
      expect(region).toBeDefined()
    })
  }

  it('[SHOULD] load the root region [WHEN] set region is called with root region id', async () => {
    fetchMockRootRegionResponse()

    await dispatchSetRegion(ROOT_REGION_ID)

    expectRegion(ROOT_REGION_ID)
  })

  it('[SHOULD] load root region and new-zealand [WHEN] set region is called with new-zealand', async () => {
    fetchMockRootRegionResponse()
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID)

    await dispatchSetRegion(NEW_ZEALAND_REGION_ID)

    expectRegion(NEW_ZEALAND_REGION_ID)
  })

  it('[SHOULD] load root region, new-zealand and wellington [WHEN] set region is called with wellington', async () => {
    fetchMockRootRegionResponse()
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID)
    fetchMockRegionResponse(WELLINGTON_REGION_ID)

    await dispatchSetRegion(WELLINGTON_REGION_ID)

    expectRegion(WELLINGTON_REGION_ID)
  })


  it('[SHOULD] load root region and australia [WHEN] set region is called with new-zealand and then australia', async () => {
    fetchMockRootRegionResponse()
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID)
    fetchMockRegionResponse(AUSTRALIA_REGION_ID)

    await dispatchSetRegion(NEW_ZEALAND_REGION_ID)
    await dispatchSetRegion(AUSTRALIA_REGION_ID)

    expectRegion(AUSTRALIA_REGION_ID)
  })

  it('[SHOULD] load root region and australia [WHEN] set region is called with new-zealand and then immediately australia, and new-zealand returns after', async () => {
    fetchMockRootRegionResponse()

    let resolveNewZealandResponse: () => void
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID, () => new Promise(resolve => {
      resolveNewZealandResponse = () => {
        resolve(createRegionResponse(NEW_ZEALAND_REGION_ID))
      }
    }))

    fetchMockRegionResponse(AUSTRALIA_REGION_ID, () => new Promise(resolve => {
      resolve(createRegionResponse(AUSTRALIA_REGION_ID))
      setTimeout(() => {
        resolveNewZealandResponse()
      }, 1)
    }))

    await Promise.all([
      dispatchSetRegion(NEW_ZEALAND_REGION_ID),
      dispatchSetRegion(AUSTRALIA_REGION_ID)
    ])

    expectRegion(AUSTRALIA_REGION_ID)
  })

  it('[SHOULD] load root region, new-zealand and auckland [WHEN] set region is called with wellington and then auckland', async () => {
    fetchMockRootRegionResponse()
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID)
    fetchMockRegionResponse(WELLINGTON_REGION_ID)
    fetchMockRegionResponse(AUCKLAND_REGION_ID)

    await dispatchSetRegion(WELLINGTON_REGION_ID)
    await dispatchSetRegion(AUCKLAND_REGION_ID)

    expectRegion(AUCKLAND_REGION_ID)
  })
})
