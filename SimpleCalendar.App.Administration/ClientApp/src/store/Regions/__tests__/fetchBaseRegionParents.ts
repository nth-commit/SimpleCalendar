import { ROOT_REGION_ID } from 'src/constants';
import configureStore from 'test-helpers/configureStore';
import { fetchMockRegionResponse, fetchMockRootRegionResponse, fetchMockSuppressNotFound } from 'test-helpers/mocks/fetch';
import { configurationActionCreators } from 'src/store/Configuration';
import { regionActionCreators } from '../';

describe('store.regions.fetchBaseRegionParents', () => {
  const { dispatch, getState } = configureStore();

  it('SHOULD not fetch any regions WHEN base is root', async () => {
    fetchMockSuppressNotFound();

    await dispatch(regionActionCreators.fetchBaseRegionParents());

    const { regions } = getState();
    expect(regions.path.length).toBe(0);
  });

  it('SHOULD fetch root WHEN base is new-zealand', async () => {
    fetchMockRootRegionResponse();
    dispatch(configurationActionCreators.update({
      baseRegionId: 'new-zealand'
    }));

    await dispatch(regionActionCreators.fetchBaseRegionParents());

    const { regions } = getState();
    expect(regions.path.length).toBe(1);
    expect(regions.path[0].id).toBe(ROOT_REGION_ID);
  });

  it('SHOULD fetch root and new-zealand WHEN base is new-zealand/wellington', async () => {
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
