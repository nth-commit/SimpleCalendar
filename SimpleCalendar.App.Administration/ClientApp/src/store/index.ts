import { regionsReducer } from './Regions';
import { configurationReducer } from './Configuration';

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer
};

export { ApplicationState } from './ApplicationState';
export { ApplicationStore } from './ApplicationStore';
export { appConnect } from './appConnect';
