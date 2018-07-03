import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../../../configureStore';
import { ApplicationStore } from '../../';
import { IRegionState, regionActionCreators } from '../';
import { IRegion } from '../../../components/services/Api';
import { configurationActionCreators, IConfigurationState } from '../../Configuration';

describe('regions', () => {
  let store: ApplicationStore;

  beforeEach(() => {
    store = configureStore(createBrowserHistory());
    store.dispatch(configurationActionCreators.update({
      rootRegionId: 'ROOT',
      api: 'http://api'
    } as IConfigurationState));
  });

  afterEach(() => {
    fetchMock.reset();
    fetchMock.restore();
  });

  it('should throw when root region has not been fetched', async () => {
    
    fetchMock.mock('http://api/regions/new-zealand', {
      body: {
        id: 'new-zealand',
        name: 'New Zealand'
      } as IRegion
    });

    let successful = true;
    try {
      await store.dispatch(regionActionCreators.getRegion('new-zealand'));
    } catch (e) {
      successful = false;
    }
    expect(successful).toBeFalsy();

    // const expectedState: IRegionsState = {
    //   region: {
    //     id: 'new-zealand',
    //     name: 'New Zealand'
    //   }
    // }

    // expect(store.getState().regions).toEqual(expectedState);
  });
});