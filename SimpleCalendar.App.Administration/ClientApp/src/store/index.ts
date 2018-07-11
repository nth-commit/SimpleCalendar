import { regionsReducer } from './Regions';
import { configurationReducer } from './Configuration';
import { authReducer } from './Auth';

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer,
  auth: authReducer
};

export * from './ApplicationState';
export * from './ApplicationStateUtility';
export * from './ApplicationStore';
export * from './ApplicationConnect';
