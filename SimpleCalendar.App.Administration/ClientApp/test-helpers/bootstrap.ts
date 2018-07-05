import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../src/configureStore';
import { ApplicationStore } from '../src/store';
import { configurationActionCreators, IConfigurationState } from 'store/Configuration';
import { API_BASE_PATH, ROOT_REGION_ID } from './constants';

export const bootstrap = (
  configuration = {
    baseRegionId: ROOT_REGION_ID,
    api: API_BASE_PATH
  } as IConfigurationState) => {

  let store: ApplicationStore;

  beforeEach(() => {
    store = configureStore(createBrowserHistory());
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