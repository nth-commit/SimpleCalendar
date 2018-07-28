import { regionsNewReducer } from './Regions'
import { membershipsReducer } from './Memberships'
import { configurationReducer } from './Configuration'
import { authReducer } from './Auth'
import { rolesReducer } from './Roles'
import { uiReducer } from './UI'

export const reducers = {
  regions: regionsNewReducer,
  memberships: membershipsReducer,
  configuration: configurationReducer,
  auth: authReducer,
  roles: rolesReducer,
  ui: uiReducer
}

export * from './ApplicationState'
export * from './ApplicationStateUtility'
export * from './ApplicationStore'
export * from './ApplicationConnect'
