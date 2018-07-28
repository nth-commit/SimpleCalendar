import { DeepPartial } from 'redux'
import { ApplicationState } from 'src/store'
import areRegionsLoadingSelector, { RegionNotInitializedError } from 'src/selectors/areRegionsLoading'
import { enumerateRegionId, RegionDictionaryEntry } from 'src/store/Regions'

describe('selectors.areRegionsLoading', () => {

  const areRegionsLoading = (state: DeepPartial<ApplicationState>) =>
    areRegionsLoadingSelector(state as ApplicationState)
  
  const expectThrows = <T extends { new (...args: any[]): InstanceType<T> }>
    (action: () => void, errorType: T, verifyError?: (e: InstanceType<T>) => void) => {
      let error: any = null
      try {
        action()
      } catch (e) {
        error = e
      }

      expect(error).not.toBeNull()
      expect(error).toBeInstanceOf(errorType)

      if (verifyError) { verifyError(error) }
    }

  it('[SHOULD] throw region not initialized [WHEN] region doesn\'t exist in state', () => {
    expectThrows(
      () => areRegionsLoading({
        regions: {
          regionId: 'ROOT',
          regionDictionary: {}
        }
      }),
      RegionNotInitializedError,
      e => {
        expect(e.regionId).toBe('ROOT')
      })
  })

  it('[SHOULD] return true [WHEN] all regions are loading', () => {
    const regionId = 'new-zealand/wellington'

    const state: DeepPartial<ApplicationState> = {
      regions: {
        regionId,
        regionDictionary: enumerateRegionId(regionId).toObject<RegionDictionaryEntry>(
          r => r,
          r => ({
            region: null,
            childRegions: null,
            timestamp: 0
          })
        )
      }
    }

    expect(areRegionsLoading(state)).toEqual(true)
  })

  it('[SHOULD] return true [WHEN] one region is loading', () => {
    const regionId = 'new-zealand/wellington'

    const state: DeepPartial<ApplicationState> = {
      regions: {
        regionId,
        regionDictionary: enumerateRegionId(regionId).toObject<RegionDictionaryEntry>(
          r => r,
          r => ({
            region: r === regionId ? null : {
              id: r,
              name: r,
              permissions: {
                canAddMemberships: {}
              }
            },
            childRegions: [],
            timestamp: 0
          })
        )
      }
    }

    expect(areRegionsLoading(state)).toEqual(true)
  })

  it('[SHOULD] return false [WHEN] all regions are loaded', () => {
    const regionId = 'new-zealand/wellington'

    const state: DeepPartial<ApplicationState> = {
      regions: {
        regionId,
        regionDictionary: enumerateRegionId(regionId).toObject<RegionDictionaryEntry>(
          r => r,
          r => ({
            region: {
              id: r,
              name: r,
              permissions: {
                canAddMemberships: {}
              }
            },
            childRegions: [],
            timestamp: 0
          })
        )
      }
    }

    expect(areRegionsLoading(state)).toEqual(false)
  })
})