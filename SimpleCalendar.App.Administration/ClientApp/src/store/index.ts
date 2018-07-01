import { AnyAction } from 'redux';
import { IRegionsState, regionsReducer } from './Regions';
import { IConfigurationState, configurationReducer } from './Configuration';

export interface IApplicationState {
  configuration: IConfigurationState;
  regions: IRegionsState;
}

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer
};

export type IAppThunkAction<TAction = AnyAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => void;
export type IAppThunkActionAsync<TAction = AnyAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => Promise<void>;
