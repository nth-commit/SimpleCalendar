import { ApplicationThunkActionAsync } from '../../'
import { FetchRegionBegin, FetchRegionComplete, FetchRegionError } from 'src/store/Regions/Actions'
import { Api } from 'src/services/Api'
import { RegionDictionaryEntry } from '../RegionsState'

const outstandingRegionRequests = new Map<string, Promise<void>>()

const isRegionExpired = (regionEntry: RegionDictionaryEntry, maxAge: number) =>
  new Date().getTime() - (regionEntry.timestamp as number) > maxAge

export function fetchRegion(regionId: string, maxAge: number = 5 * 60 * 1000): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { auth, regions } = getState()

    const regionEntry = regions.regionDictionary[regionId]
    if (regionEntry && !isRegionExpired(regionEntry, maxAge)) {
      const outstandingRegionRequest = outstandingRegionRequests.get(regionId)
      if (outstandingRegionRequest) {
        await outstandingRegionRequest
      }
      return
    }

    dispatch({ ...new FetchRegionBegin(regionId) })

    const api = new Api(auth.accessToken)
    const regionPromise = Promise.all([
      api.getRegion(regionId),
      api.getRegions(regionId)
    ])
    outstandingRegionRequests.set(regionId, regionPromise as any)

    try {
      const [region, childRegions] = await regionPromise
      dispatch({ ...new FetchRegionComplete(region, childRegions) })
    } catch (e) {
      dispatch({ ...new FetchRegionError(e) })
    } finally {
      outstandingRegionRequests.delete(regionId)
    }
  }
}
