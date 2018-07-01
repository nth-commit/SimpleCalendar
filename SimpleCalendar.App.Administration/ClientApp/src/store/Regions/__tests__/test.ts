import { Store } from 'redux';
import * as fetchMock from 'fetch-mock';
import { createBrowserHistory } from 'history';
import configureStore from '../../../configureStore';
import { IRegionsState, regionActionCreators } from '../';
import {
  setConfiguration as setApiConfiguration,
  clearConfiguration as clearApiConfiguration,
  IRegion
} from '../../../components/services/Api';

describe('regions', () => {
  let store: Store;

  beforeEach(() => {
    store = configureStore(createBrowserHistory());
    setApiConfiguration({ 'baseUri': 'http://api' });
  })

  afterEach(() => {
    clearApiConfiguration();
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

    await regionActionCreators.getRegion('new-zealand')(store.dispatch, store.getState);

    const expectedState: IRegionsState = {
      region: {
        id: 'new-zealand',
        name: 'New Zealand'
      }
    }

    expect(store.getState().regions).toEqual(expectedState);
  });
});