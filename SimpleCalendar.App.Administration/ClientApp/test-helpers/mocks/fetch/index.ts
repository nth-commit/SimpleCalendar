import * as _fetchMock from 'fetch-mock'
import { ROOT_REGION_ID } from 'src/constants'
import { API_BASE_PATH } from '../../constants'
import { IRegion, IRegionMembership, IRegionMembershipCreate } from 'src/services/Api'

const REGIONS_API = API_BASE_PATH + '/regions'
const REGION_MEMBERSHIPS_API = API_BASE_PATH + '/regionmemberships'

const getRegionLocation = (regionId: string) => `${REGIONS_API}/${regionId}`
const getRegionMembershipsLocation = (regionMembershipId: string) => `${REGION_MEMBERSHIPS_API}/${regionMembershipId}`

export const createRegionResponse = (regionId: string) => ({
  body: {
    id: regionId,
    name: ''
  } as IRegion
})

export interface ResponseOptions {
  delay?: number
}

export const fetchMockRootRegionResponse = () =>
  fetchMockRegionResponse(ROOT_REGION_ID)

export const fetchMockRegionResponse = (regionId: string, response?: _fetchMock.MockResponse | _fetchMock.MockResponseFunction) => {
  const location = getRegionLocation(regionId)

  fetchMock.mock({
    matcher: url => location === url,
    name: location,
    response: response || createRegionResponse(regionId),
    overwriteRoutes: true
  })

  return location
}

export const fetchMockRegionMembershipsByRegionResponse = (regionId: string, regionMemberships: IRegionMembership[]): string => {
  const location = `${REGION_MEMBERSHIPS_API}?regionId=${regionId}`

  fetchMock.mock({
    matcher: url => location === url,
    name: location,
    response: {
      body: regionMemberships,
      status: 200
    },
    overwriteRoutes: true
  })

  return location
}

export const fetchMockRegionMembershipCreateResponse = (regionMembershipCreate: IRegionMembershipCreate): string => {
  const name = 'create-regionmemberships'

  let createdId = 0

  fetchMock.mock({
    matcher: (url, opts) => {
      return url === REGION_MEMBERSHIPS_API && opts.method === 'POST'
    },
    response: (url, opts) => {
      return {
        body: {
          ...regionMembershipCreate,
          id: '' + createdId++
        } as IRegionMembership,
        status: 204
      }
    }
  })

  return name
}

export const fetchMockRegionMembershipDelete = (regionMembershipId: string): string => {
  const location = getRegionMembershipsLocation(regionMembershipId)

  fetchMock.mock({
    matcher: (url, opts) => url === location && opts.method === 'DELETE',
    name: location,
    response: {
      status: 200
    },
    overwriteRoutes: true
  })

  return location
}

export const fetchMockThrowNotFound = () => {
  fetchMock.get('*', 404)
}

export const fetchMockSuppressNotFound = () => {
  fetchMock.get('*', {
    body: {},
    status: 200
  })
}

export const fetchMockRegionQueryResponse = () => {
  fetchMock.get(REGIONS_API, {
    body: []
  })
}


export const fetchMock = _fetchMock