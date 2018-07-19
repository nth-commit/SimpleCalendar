import { RouterState } from 'connected-react-router';
import { RegionState } from './Regions';
import { IConfigurationState } from './Configuration';
import { AuthState } from './Auth';
import { RoleState } from './Roles';

export interface ApplicationState {
  router: RouterState;
  configuration: IConfigurationState;
  regions: RegionState;
  auth: AuthState
  roles: RoleState;
}