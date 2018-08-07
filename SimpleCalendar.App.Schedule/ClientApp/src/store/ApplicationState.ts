import { RouterState } from 'connected-react-router'
import { IConfigurationState } from './Configuration'
import { AuthState } from './Auth'
import { RegionState } from './Region'
import { UIState } from './UI'

export interface ApplicationState {
  router: RouterState
  configuration: IConfigurationState
  auth: AuthState
  region: RegionState
  ui: UIState
}