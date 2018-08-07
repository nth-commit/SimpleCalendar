import { configurationReducer } from './Configuration'
import { authReducer } from './Auth'
import { regionReducer } from './Region'
import { uiReducer } from './UI'

export const reducers = {
  configuration: configurationReducer,
  auth: authReducer,
  region: regionReducer,
  ui: uiReducer
}

export * from './ApplicationState'
export * from './ApplicationStore'
export * from './ApplicationConnect'
