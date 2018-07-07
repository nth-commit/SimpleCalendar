import { regionActionCreators } from '../';
import { ROOT_REGION_ID } from 'src/constants';
import configureStore from 'test-helpers/configureStore';
import { fetchMockRegionResponse } from 'test-helpers/mocks/fetch';

describe('regions.fetchRegions', () => {
  const { dispatch, getState } = configureStore();

  it('should fetch only root when root requested', async () => {
    fetchMockRegionResponse(ROOT_REGION_ID);

    await dispatch(regionActionCreators.fetchRegions(ROOT_REGION_ID));

    const { regions } = getState();
    expect(regions.path.length).toBe(1);
    expect(regions.path[0].id).toBe(ROOT_REGION_ID);
  });

  it('should fetch all regions in hierarchy', async () => {
    const regionIdComponents = ['new-zealand', 'wellington', 'mount-victoria']
    const regionIds = [
      ROOT_REGION_ID,
      ...Array
        .from({ length: regionIdComponents.length })
        .map((x, i) => regionIdComponents.slice(0, i + 1).join('/'))
    ];

    regionIds.forEach(r => fetchMockRegionResponse(r));

    await dispatch(regionActionCreators.fetchRegions(regionIds[3]));

    const actualRegionIds = getState().regions.path.map(r => r.id);
    expect(actualRegionIds).toEqual(regionIds);
  });
});
