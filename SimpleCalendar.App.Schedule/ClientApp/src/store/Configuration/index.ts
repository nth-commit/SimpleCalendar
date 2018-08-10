import { Reducer, Action, DeepPartial } from 'redux'
import { setConfiguration as setAuthConfiguration } from 'src/services/Auth'
import { setConfiguration as setApiConfiguration } from 'src/services/Api'
import { ApplicationThunkAction } from '../'

export interface IConfigurationState {
  regionId: string
  host: string
  api: string
  auth: {
    clientId: string
    domain: string
  }
}

enum ConfigurationActionTypes {
  UPDATE = '[Configuration] UPDATE'
}

class UpdateConfiguration implements Action {
  readonly type = ConfigurationActionTypes.UPDATE
  constructor(public configuration: IConfigurationState) { }
}

declare type ConfigurationAction = UpdateConfiguration

const DEFAULT_STATE = {} as IConfigurationState

export const configurationReducer: Reducer<IConfigurationState, ConfigurationAction> = (state = DEFAULT_STATE, action): IConfigurationState => {
    switch (action.type) {
      case ConfigurationActionTypes.UPDATE:
        return Object.assign({}, state, action.configuration)
      default:
        return state
    }
}

class ConfigurationActionCreators {

  update(configuration: DeepPartial<IConfigurationState>): ApplicationThunkAction {
    return (dispatch) => {

      const { host, auth, api } = configuration

      if (host && auth) {
        const { clientId, domain } = auth
        if (!clientId || !domain) {
          throw new Error('Invalid update of auth')
        }

        const redirectUri = new URL(host)
        redirectUri.pathname = 'callback'

        setAuthConfiguration({
          domain,
          clientId,
          redirectUri: redirectUri.toString()
        })
      }

      if (api) {
        setApiConfiguration({
          baseUri: api
        })
      }

      dispatch(new UpdateConfiguration(configuration as IConfigurationState))
    }
  }
}

export const configurationActionCreators = new ConfigurationActionCreators()