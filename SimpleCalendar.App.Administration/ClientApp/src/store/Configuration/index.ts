import { Reducer, Action } from 'redux';
import { setConfiguration as setAuthConfiguration } from 'src/components/services/Auth';
import { setConfiguration as setApiConfiguration } from 'src/components/services/Api';
import { ApplicationThunkAction } from '../';

export interface IConfigurationState {
  baseRegionId: string;
  host: string;
  api: string;
  auth: {
    clientId: string;
    domain: string;
  }
}

enum ConfigurationActionTypes {
  UPDATE = '[Configuration] UPATE'
}

class UpdateConfiguration implements Action {
  readonly type = ConfigurationActionTypes.UPDATE;
  constructor(public configuration: IConfigurationState) { }
}

declare type ConfigurationAction = UpdateConfiguration;

export const configurationReducer: Reducer = (
  state: IConfigurationState = {} as IConfigurationState,
  action: ConfigurationAction): IConfigurationState => {
    switch (action.type) {
      case ConfigurationActionTypes.UPDATE:
        return Object.assign({}, state, action.configuration);
      default:
        return state;
    }
}

class ConfigurationActionCreators {

  update(configuration: IConfigurationState): ApplicationThunkAction {
    return (dispatch, getState) => {

      dispatch({ ...new UpdateConfiguration(configuration) });

      if (configuration.host && configuration.auth) {
        const redirectUri = new URL(configuration.host);
        redirectUri.pathname = 'callback';

        setAuthConfiguration({
          domain: configuration.auth.domain,
          clientId: configuration.auth.clientId,
          redirectUri: redirectUri.toString()
        });
      }

      setApiConfiguration({
        baseUri: configuration.api
      });
    }
  }
}

export const configurationActionCreators = new ConfigurationActionCreators();