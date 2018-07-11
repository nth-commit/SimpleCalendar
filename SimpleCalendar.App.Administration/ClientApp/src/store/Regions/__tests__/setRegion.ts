import { ROOT_REGION_ID } from 'src/constants';
import configureStore from 'test-helpers/configureStore';
import { fetchMockRootRegionResponse, fetchMockRegionResponse, createRegionResponse } from 'test-helpers/mocks/fetch';
import { regionActionCreators, RegionPathComponentValue } from '../';

describe('store.regions.setRegion', () => {
  const NEW_ZEALAND_REGION_ID = 'new-zealand';
  const WELLINGTON_REGION_ID = 'new-zealand/wellington';
  const AUCKLAND_REGION_ID = 'new-zealand/auckland';
  const AUSTRALIA_REGION_ID = 'australia';
  const { dispatch, getState } = configureStore();

  const dispatchSetRegion = async (regionId: string) => {
    const setActivePromise = dispatch(regionActionCreators.setRegion(regionId));
    expect(getState().regions.regionId).toBe(regionId);
    await setActivePromise;
  };

  const getRegionLevel = (regionId: string) =>
    regionId === ROOT_REGION_ID ? 0 : regionId.split('/').length;

  const expectRegion = (regionId: string) => {
    const regionLevel = getRegionLevel(regionId);
    const regionPath = getState().regions.path;
    expect(regionPath.length).toBe(regionLevel + 1);
    regionPath.forEach(pathComponent => {
      expect(pathComponent).toBeDefined();
      expect(pathComponent.value).not.toBeNull();
    });
    
    const regionPathComponent = regionPath[regionLevel];
    expect(regionPathComponent).toBeDefined();

    expect(regionPathComponent.id).toBe(regionId);
    expect(regionPathComponent.value).not.toBe(null);

    const regionPathComponentValue = regionPathComponent.value as RegionPathComponentValue;
    expect(regionPathComponentValue.region).not.toBe(null);
    expect(regionPathComponentValue.region.id).toBe(regionId);
  };

  it(`[SHOULD] load root region
      [WHEN] set region is called with root region id`,
  async () => {
    fetchMockRootRegionResponse();

    await dispatchSetRegion(ROOT_REGION_ID);

    expectRegion(ROOT_REGION_ID);
  });

  it(`[SHOULD] load root region and new-zealand
      [WHEN] set region is called with new-zealand`,
  async () => {
    fetchMockRootRegionResponse();
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID);

    await dispatchSetRegion(NEW_ZEALAND_REGION_ID);

    expectRegion(NEW_ZEALAND_REGION_ID);
  });

  it(`[SHOULD] load root region, new-zealand and wellington
      [WHEN] set region is called with wellington`,
  async () => {
    fetchMockRootRegionResponse();
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID);
    fetchMockRegionResponse(WELLINGTON_REGION_ID);

    await dispatchSetRegion(WELLINGTON_REGION_ID);

    expectRegion(WELLINGTON_REGION_ID);
  });


  it(`[SHOULD] load root region and australia
      [WHEN] set region is called with new-zealand and then australia`,
  async () => {
    fetchMockRootRegionResponse();
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID);
    fetchMockRegionResponse(AUSTRALIA_REGION_ID);

    await dispatchSetRegion(NEW_ZEALAND_REGION_ID);
    await dispatchSetRegion(AUSTRALIA_REGION_ID)

    expectRegion(AUSTRALIA_REGION_ID);
  });

  it(`[SHOULD] load root region and australia 2
      [WHEN] set region is called with new-zealand and then immediately australia, and new-zealand returns after`,
  async () => {
    fetchMockRootRegionResponse();

    let resolveNewZealandResponse: () => void;
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID, () => new Promise(resolve => {
      resolveNewZealandResponse = () => {
        resolve(createRegionResponse(NEW_ZEALAND_REGION_ID));
      };
    }));

    fetchMockRegionResponse(AUSTRALIA_REGION_ID, () => new Promise(resolve => {
      resolve(createRegionResponse(AUSTRALIA_REGION_ID));
      setTimeout(() => {
        resolveNewZealandResponse();
      }, 1);
    }));

    await Promise.all([
      dispatchSetRegion(NEW_ZEALAND_REGION_ID),
      dispatchSetRegion(AUSTRALIA_REGION_ID)
    ]);

    expectRegion(AUSTRALIA_REGION_ID);
  });

  it(`[SHOULD] load root region, new-zealand and auckland
      [WHEN] set region is called with wellington and then auckland`,
  async () => {
    fetchMockRootRegionResponse();
    fetchMockRegionResponse(NEW_ZEALAND_REGION_ID);
    fetchMockRegionResponse(WELLINGTON_REGION_ID);
    fetchMockRegionResponse(AUCKLAND_REGION_ID);

    await dispatchSetRegion(WELLINGTON_REGION_ID);
    await dispatchSetRegion(AUCKLAND_REGION_ID);

    expectRegion(AUCKLAND_REGION_ID);
  });
});
