import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../src/configureStore';
import { ApplicationStore } from '../src/store';
import { configurationActionCreators, IConfigurationState } from 'store/Configuration';
import { API_BASE_PATH, ROOT_REGION_ID } from './constants';

interface ApplicationStoreContainer {
  store: ApplicationStore;
}

export const bootstrap = (
  configuration = { baseRegionId: ROOT_REGION_ID, api: API_BASE_PATH } as IConfigurationState) => {

  const storeContainer = {} as ApplicationStoreContainer;

  beforeEach(() => {
    storeContainer.store = configureStore(createBrowserHistory());
    storeContainer.store.dispatch(configurationActionCreators.update(configuration));
  });

  afterEach(() => {
    fetchMock.reset();
    fetchMock.restore();
  });

  return {
    dispatch: (...args) => (storeContainer.store as any).dispatch(...args),
    getState: (...args) => (storeContainer.store as any).getState(...args)
  } as ApplicationStore
}