import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStoreReal from '../src/configureStore';
import { ApplicationStore } from '../src/store';
import { configurationActionCreators, IConfigurationState } from 'store/Configuration';
import { API_BASE_PATH, ROOT_REGION_ID } from './constants';

const DEFAULT_CONFIGURATION = {
  baseRegionId: ROOT_REGION_ID,
  api: API_BASE_PATH
} as IConfigurationState;

export default function configureStore(configuration = DEFAULT_CONFIGURATION) {
  let store: ApplicationStore;

  beforeEach(() => {
    store = configureStoreReal(createBrowserHistory());
    store.dispatch(configurationActionCreators.update(configuration));
  });

  afterEach(() => {
    fetchMock.reset();
    fetchMock.restore();
  });

  return {
    dispatch: (...args) => (store as any).dispatch(...args),
    getState: (...args) => (store as any).getState(...args)
  } as ApplicationStore;
}