import { RouterState } from 'connected-react-router'
import { IConfigurationState } from './Configuration'
import { RegionsState } from './Regions'
import { MembershipsState } from './Memberships'
import { AuthState } from './Auth'
import { RoleState } from './Roles'
import { UIState } from './UI'

export interface ApplicationState {
  router: RouterState
  configuration: IConfigurationState
  regions: RegionsState
  memberships: MembershipsState
  auth: AuthState
  roles: RoleState
  ui: UIState
}