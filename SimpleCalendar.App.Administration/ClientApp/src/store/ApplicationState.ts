import { RouterState } from 'connected-react-router';
import { RegionState } from './Regions';
import { IConfigurationState } from './Configuration';

export interface ApplicationState {
  router: RouterState;
  configuration: IConfigurationState;
  regions: RegionState;
}