import * as _fetchMock from 'fetch-mock';
import { API_BASE_PATH } from '../../constants';
import { IRegion } from 'components/services/Api';

const REGIONS_API = API_BASE_PATH + '/regions';

const getRegionLocation = (regionId: string) => `${REGIONS_API}/${regionId}`;

const createRegionResponse = (regionId: string) => ({
  body: {
    id: regionId,
    name: ''
  } as IRegion
});

export const fetchMockRegionResponse = (regionId: string) => {
  fetchMock.mock(getRegionLocation(regionId), createRegionResponse(regionId));
}

export const fetchMockSuppressNotFound = () => {
  fetchMock.get('*', 404);
}


export const fetchMock = _fetchMock;