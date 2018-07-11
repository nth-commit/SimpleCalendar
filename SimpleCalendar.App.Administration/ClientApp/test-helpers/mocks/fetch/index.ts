import * as _fetchMock from 'fetch-mock';
import { ROOT_REGION_ID } from 'src/constants';
import { API_BASE_PATH } from '../../constants';
import { IRegion } from 'src/services/Api';

const REGIONS_API = API_BASE_PATH + '/regions';

const getRegionLocation = (regionId: string) => `${REGIONS_API}/${regionId}`;

export const createRegionResponse = (regionId: string) => ({
  body: {
    id: regionId,
    name: ''
  } as IRegion
});

export interface ResponseOptions {
  delay?: number;
}

export const fetchMockRootRegionResponse = () =>
  fetchMockRegionResponse(ROOT_REGION_ID);

export const fetchMockRegionResponse = (regionId: string, response?: _fetchMock.MockResponse | _fetchMock.MockResponseFunction) => {
  const location = getRegionLocation(regionId);

  fetchMock.mock(
    location,
    response || createRegionResponse(regionId)
  );
}

export const fetchMockSuppressNotFound = () => {
  fetchMock.get('*', 404);
}


export const fetchMock = _fetchMock;