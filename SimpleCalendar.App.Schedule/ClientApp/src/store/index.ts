import { configurationReducer } from './Configuration'
import { authReducer } from './Auth'
import { uiReducer } from './UI'

export const reducers = {
  configuration: configurationReducer,
  auth: authReducer,
  ui: uiReducer
}

export * from './ApplicationState'
export * from './ApplicationStore'
export * from './ApplicationConnect'
