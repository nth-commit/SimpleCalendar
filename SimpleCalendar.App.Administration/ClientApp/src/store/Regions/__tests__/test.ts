import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../../../configureStore';
import { ApplicationStore } from '../../';
import { IRegionsState, regionActionCreators } from '../';
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

  it('region test', async () => {
    
    fetchMock.mock('http://api/regions/new-zealand', {
      body: {
        id: 'new-zealand',
        name: 'New Zealand'
      } as IRegion
    });

    await store.dispatch(regionActionCreators.getRegion('new-zealand'))

    const expectedState: IRegionsState = {
      region: {
        id: 'new-zealand',
        name: 'New Zealand'
      }
    }

    expect(store.getState().regions).toEqual(expectedState);
  });
});