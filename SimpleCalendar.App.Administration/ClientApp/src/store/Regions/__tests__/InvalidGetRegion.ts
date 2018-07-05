import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../../../configureStore';
import { ApplicationStore } from '../../';
import { regionActionCreators } from '../';
import { IRegion } from 'components/services/Api';
import { configurationActionCreators, IConfigurationState } from 'store/Configuration';

const ROOT_REGION_ID = 'ROOT';
const API_BASE_PATH = 'http://api';
const REGIONS_API = API_BASE_PATH + '/regions';

const getRegionLocation = (regionId: string) => `${REGIONS_API}/${regionId}`;

const createRegionResponse = (regionId: string) => ({
  body: {
    id: regionId,
    name: ''
  } as IRegion
});

const fetchMockRegionResponse = (regionId: string) => {
  fetchMock.mock(getRegionLocation(regionId), createRegionResponse(regionId));
}

const fetchMockSuppressNotFound = () => {
  fetchMock.get('*', 404);
}

const expectThrowsAsync = async (action: () => Promise<any>) => {
  let successful = false;
  try {
    await action();
  } catch {
    successful = true;
  }
  expect(successful).toBe(true);
}

describe('regions', () => {
  let store: ApplicationStore;

  beforeEach(() => {
    store = configureStore(createBrowserHistory());
    store.dispatch(configurationActionCreators.update({
      baseRegionId: ROOT_REGION_ID,
      api: API_BASE_PATH
    } as IConfigurationState));
  });

  afterEach(() => {
    fetchMock.reset();
    fetchMock.restore();
  });

  it('should throw when root region has not been fetched', async () => {
    fetchMockRegionResponse('new-zealand');

    await expectThrowsAsync(async () => await store.dispatch(regionActionCreators.getRegion('new-zealand')))
  });

  it('should throw when region not found', async () => {
    fetchMockRegionResponse('ROOT');
    fetchMockSuppressNotFound();

    await store.dispatch(regionActionCreators.getRegion('ROOT'));
    await expectThrowsAsync(async () => await store.dispatch(regionActionCreators.getRegion('non-existent-region')));
  });

  it('should throw when parent region has not been fetched', async () => {
    fetchMockRegionResponse('ROOT');
    fetchMockRegionResponse('new-zealand/wellington');

    await store.dispatch(regionActionCreators.getRegion('ROOT'));
    await expectThrowsAsync(async () => await store.dispatch(regionActionCreators.getRegion('new-zealand/wellington')));
  });
});