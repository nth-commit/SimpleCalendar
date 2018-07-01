import { Reducer, Action } from 'redux';
import { IAppThunkAction } from '../';
import { setConfiguration as setAuthConfiguration } from '../../components/services/Auth';
import { setConfiguration as setApiConfiguration } from '../../components/services/Api';

export interface IConfigurationState {
  rootRegionId: string;
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
        return action.configuration;
      default:
        return state;
    }
}

export const configurationActionCreators = {
  update: (configuration: IConfigurationState): IAppThunkAction => (dispatch, getState): void => {
    dispatch({ ...new UpdateConfiguration(configuration) });

    const redirectUri = new URL(configuration.host);
    redirectUri.pathname = 'callback';

    setAuthConfiguration({
      domain: configuration.auth.domain,
      clientId: configuration.auth.clientId,
      redirectUri: redirectUri.toString()
    });

    setApiConfiguration({
      baseUri: configuration.api
    });
  }
};