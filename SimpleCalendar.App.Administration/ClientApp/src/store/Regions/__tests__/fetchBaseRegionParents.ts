import { ROOT_REGION_ID } from 'src/constants';
import configureStore from 'test-helpers/configureStore';
import { fetchMockRegionResponse, fetchMockRootRegionResponse, fetchMockSuppressNotFound } from 'test-helpers/mocks/fetch';
import { configurationActionCreators } from 'src/store/Configuration';
import { regionActionCreators } from '../';

describe('store.regions.fetchRegionsAboveBase', () => {
  const { dispatch, getState } = configureStore();

  it('should not fetch any regions when base is root', async () => {
    fetchMockSuppressNotFound();

    await dispatch(regionActionCreators.fetchBaseRegionParents());

    const { regions } = getState();
    expect(regions.path.length).toBe(0);
  });

  it('should fetch root region when base is one above root', async () => {
    fetchMockRootRegionResponse();
    dispatch(configurationActionCreators.update({
      baseRegionId: 'new-zealand'
    }));

    await dispatch(regionActionCreators.fetchBaseRegionParents());

    const { regions } = getState();
    expect(regions.path.length).toBe(1);
    expect(regions.path[0].id).toBe(ROOT_REGION_ID);
  });

  it('should fetch regions when base is two above root', async () => {
    fetchMockRootRegionResponse();
    fetchMockRegionResponse('new-zealand');
    dispatch(configurationActionCreators.update({
      baseRegionId: 'new-zealand/wellington'
    }));

    await dispatch(regionActionCreators.fetchBaseRegionParents());

    const { regions } = getState();
    expect(regions.path.length).toBe(2);
    expect(regions.path.map(r => r.id)).toEqual([ROOT_REGION_ID, 'new-zealand']);
  });
});
