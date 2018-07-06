import { configurationActionCreators, IConfigurationState } from '../';
import configureStore from 'test-helpers/configureStore';

describe('configuration.update', () => {
  const { dispatch, getState } = configureStore();

  it('should allow sparse update of configuration', async () => {
    const initialApiConfiguration = getState().configuration.api;
    expect(initialApiConfiguration).toBeTruthy();

    dispatch(configurationActionCreators.update({
      baseRegionId: 'new-region-id'
    } as IConfigurationState));

    const apiConfiguration = getState().configuration.api;
    expect(apiConfiguration).toBe(initialApiConfiguration);
  });
});