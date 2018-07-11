import { regionsReducer } from './Regions';
import { configurationReducer } from './Configuration';

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer
};

export * from './ApplicationState';
export * from './ApplicationStateUtility';
export * from './ApplicationStore';
export * from './ApplicationConnect';
