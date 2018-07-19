import { regionsReducer } from './Regions';
import { configurationReducer } from './Configuration';
import { authReducer } from './Auth';
import { rolesReducer } from './Roles';

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer,
  auth: authReducer,
  roles: rolesReducer
};

export * from './ApplicationState';
export * from './ApplicationStateUtility';
export * from './ApplicationStore';
export * from './ApplicationConnect';
