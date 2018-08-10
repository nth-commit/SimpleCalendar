import { Reducer } from 'redux'
import { ApplicationState } from './ApplicationState'
import { configurationReducer } from './Configuration'
import { authReducer } from './Auth'
import { regionReducer } from './Region'
import { eventsReducer } from './Events'
import { uiReducer } from './UI'

export type ApplicationReducers = {
  [P in keyof ApplicationState]: Reducer<ApplicationState[P]>
}

export const reducers: ApplicationReducers = {
  auth: authReducer,
  configuration: configurationReducer,
  region: regionReducer,
  events: eventsReducer,
  ui: uiReducer
}

export * from './ApplicationState'
export * from './ApplicationStore'
export * from './ApplicationConnect'
export * from './ErrorAction'