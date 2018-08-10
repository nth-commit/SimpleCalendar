import { IConfigurationState } from './Configuration'
import { AuthState } from './Auth'
import { RegionState } from './Region'
import { EventsState } from './Events'
import { UIState } from './UI'

export interface ApplicationState {
  configuration: IConfigurationState
  auth: AuthState
  region: RegionState
  events: EventsState
  ui: UIState
}