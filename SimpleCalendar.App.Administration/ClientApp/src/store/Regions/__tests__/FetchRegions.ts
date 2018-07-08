import { ROOT_REGION_ID } from 'src/constants';
import { configurationActionCreators } from 'src/store/Configuration';
import configureStore from 'test-helpers/configureStore';
import { fetchMockRegionResponse } from 'test-helpers/mocks/fetch';
import { regionActionCreators } from '../';
import { expectThrowsAsync } from 'test-helpers/expect-helpers';

describe('store.regions.fetchRegions', () => {
  const { dispatch, getState } = configureStore();

  const dispatchFetchRegions = (regionPath: string) =>
    dispatch(regionActionCreators.fetchRegions(regionPath));

  const expectRegions = async (requestedRegionPath: string, baseRegionId: string, expectedRegionIds: string[]): Promise<void> => {
    expectedRegionIds.forEach(r => fetchMockRegionResponse(r));
    dispatch(configurationActionCreators.update({ baseRegionId }));

    await dispatchFetchRegions(requestedRegionPath);

    const actualRegionIds = getState().regions.path.map(r => r.id);
    expect(actualRegionIds).toEqual(expectedRegionIds);
  }

  it('SHOULD throw InvalidRegionPath WHEN requested regionPath is not valid', async () => {
    await expectThrowsAsync(() => dispatchFetchRegions('not-a-valid-path'));
  });

  it(`SHOULD get root WHEN base is root and / is requested`, async () => {
    await expectRegions(
      '/',
      ROOT_REGION_ID,
      [ROOT_REGION_ID]
    );
  });

  it(`SHOULD get ROOT and new-zealand WHEN base is ROOT and /new-zealand is requested`, async () => {
    await expectRegions(
      '/new-zealand',
      ROOT_REGION_ID,
      [ROOT_REGION_ID, 'new-zealand']
    );
  });

  it(`SHOULD get ROOT and australia WHEN base is ROOT and /australia is requested`, async () => {
    await expectRegions(
      '/australia',
      ROOT_REGION_ID,
      [ROOT_REGION_ID, 'australia']
    );
  });

  it(`SHOULD get ROOT, new-zealand and new-zealand/wellington WHEN base is ROOT and /new-zealand/wellington is requested`, async () => {
    await expectRegions(
      '/new-zealand/wellington',
      ROOT_REGION_ID,
      [ROOT_REGION_ID, 'new-zealand', 'new-zealand/wellington']
    );
  });

  it(`SHOULD get ROOT, new-zealand, new-zealand/wellington and new-zealand/wellington/mount-vic WHEN base is ROOT and /new-zealand/wellington/mount-vic is requested`, async () => {
    await expectRegions(
      '/new-zealand/wellington/mount-vic',
      ROOT_REGION_ID,
      [ROOT_REGION_ID, 'new-zealand', 'new-zealand/wellington', 'new-zealand/wellington/mount-vic']
    );
  });

  it(`SHOULD get ROOT and new-zealand WHEN base is new-zealand and / is requested`, async () => {
    await expectRegions(
      '/',
      'new-zealand',
      [ROOT_REGION_ID, 'new-zealand']
    );
  });

  it(`SHOULD get ROOT, new-zealand and new-zealand/wellington WHEN base is new-zealand and /wellington is requested`, async () => {
    await expectRegions(
      '/wellington',
      'new-zealand',
      [ROOT_REGION_ID, 'new-zealand', 'new-zealand/wellington']
    );
  });

  it(`SHOULD get ROOT, new-zealand and new-zealand/wellington WHEN base is wellington and / is requested`, async () => {
    await expectRegions(
      '/',
      'new-zealand/wellington',
      [ROOT_REGION_ID, 'new-zealand', 'new-zealand/wellington']
    );
  });

  it(`SHOULD get ROOT, new-zealand, new-zealand/wellington and new-zealand/wellington/mount-vic WHEN base is mount-vic and / is requested`, async () => {
    await expectRegions(
      '/',
      'new-zealand/wellington/mount-vic',
      [ROOT_REGION_ID, 'new-zealand', 'new-zealand/wellington', 'new-zealand/wellington/mount-vic']
    );
  });
});
