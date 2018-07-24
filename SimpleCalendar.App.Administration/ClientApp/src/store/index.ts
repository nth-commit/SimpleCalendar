import { regionsReducer } from './Regions'
import { configurationReducer } from './Configuration'
import { authReducer } from './Auth'
import { rolesReducer } from './Roles'
import { uiReducer } from './UI'

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer,
  auth: authReducer,
  roles: rolesReducer,
  ui: uiReducer
}

export * from './ApplicationState'
export * from './ApplicationStateUtility'
export * from './ApplicationStore'
export * from './ApplicationConnect'
