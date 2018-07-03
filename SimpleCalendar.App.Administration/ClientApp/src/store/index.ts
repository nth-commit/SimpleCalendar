import { AnyAction, Store } from 'redux';
import { ThunkAction } from 'redux-thunk';
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

export type ApplicationThunkAction = ThunkAction<void, IApplicationState, {}, AnyAction>;

export type ApplicationStore = Store<IApplicationState, AnyAction> & { dispatch(thunk: ApplicationThunkAction )};
