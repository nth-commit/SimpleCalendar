import { ROOT_REGION_ID } from 'src/constants';
import { configurationActionCreators, IConfigurationState } from '../../Configuration';
import configureStore from 'test-helpers/configureStore';
import { expectThrowsAsync } from 'test-helpers/expect-helpers';
import { fetchMockRegionResponse, fetchMockSuppressNotFound } from 'test-helpers/mocks/fetch';
import { regionActionCreators } from '../';

describe('invalid fetch region', () => {
  const { dispatch } = configureStore();

  it('should throw when root region has not been fetched', async () => {
    fetchMockRegionResponse('new-zealand');

    await expectThrowsAsync(() => dispatch(regionActionCreators.fetchRegion('new-zealand')))
  });

  it('should throw when root region has not been fetched when base is not root', async () => {
    dispatch(configurationActionCreators.update({
      baseRegionId: 'new-zealand'
    } as IConfigurationState));

    fetchMockRegionResponse('new-zealand');

    await expectThrowsAsync(() => dispatch(regionActionCreators.fetchRegion('new-zealand')))
  });

  it('should throw when region not found', async () => {
    fetchMockRegionResponse(ROOT_REGION_ID);
    fetchMockSuppressNotFound();

    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID));
    await expectThrowsAsync(() => dispatch(regionActionCreators.fetchRegion('non-existent-region')));
  });

  it('should throw when parent region has not been fetched', async () => {
    fetchMockRegionResponse(ROOT_REGION_ID);
    fetchMockRegionResponse('new-zealand/wellington');

    await dispatch(regionActionCreators.fetchRegion(ROOT_REGION_ID));
    await expectThrowsAsync(() => dispatch(regionActionCreators.fetchRegion('new-zealand/wellington')));
  });
});
