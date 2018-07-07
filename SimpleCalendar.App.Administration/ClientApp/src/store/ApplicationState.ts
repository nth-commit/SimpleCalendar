import { RouterState } from 'connected-react-router';
import { IRegionState } from './Regions';
import { IConfigurationState } from './Configuration';

export interface ApplicationState {
  router: RouterState;
  configuration: IConfigurationState;
  regions: IRegionState;
}