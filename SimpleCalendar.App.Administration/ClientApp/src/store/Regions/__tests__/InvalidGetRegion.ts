import { regionActionCreators } from '../';
import { bootstrap } from 'test-helpers/bootstrap';
import { expectThrowsAsync } from 'test-helpers/expect-helpers';
import { fetchMockRegionResponse, fetchMockSuppressNotFound } from 'test-helpers/mocks/fetch';

describe('regions', () => {
  const { dispatch } = bootstrap();

  it('should throw when root region has not been fetched', async () => {
    fetchMockRegionResponse('new-zealand');

    await expectThrowsAsync(() => dispatch(regionActionCreators.getRegion('new-zealand')))
  });

  it('should throw when region not found', async () => {
    fetchMockRegionResponse('ROOT');
    fetchMockSuppressNotFound();

    await dispatch(regionActionCreators.getRegion('ROOT'));
    await expectThrowsAsync(() => dispatch(regionActionCreators.getRegion('non-existent-region')));
  });

  it('should throw when parent region has not been fetched', async () => {
    fetchMockRegionResponse('ROOT');
    fetchMockRegionResponse('new-zealand/wellington');

    await dispatch(regionActionCreators.getRegion('ROOT'));
    await expectThrowsAsync(() => dispatch(regionActionCreators.getRegion('new-zealand/wellington')));
  });
});
