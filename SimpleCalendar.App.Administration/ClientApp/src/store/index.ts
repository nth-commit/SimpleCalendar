import { AnyAction, Store } from 'redux';
import { ThunkAction } from 'redux-thunk';
import { IRegionState, regionsReducer } from './Regions';
import { IConfigurationState, configurationReducer } from './Configuration';

export interface IApplicationState {
  configuration: IConfigurationState;
  regions: IRegionState;
}

export const reducers = {
  configuration: configurationReducer,
  regions: regionsReducer
};

export type ApplicationThunkAction = ThunkAction<void, IApplicationState, {}, AnyAction>;
export type ApplicationThunkActionAsync = ThunkAction<Promise<void>, IApplicationState, {}, AnyAction>;

export type ApplicationStore = Store<IApplicationState, AnyAction> & {
  dispatch(thunkAsync: ApplicationThunkActionAsync): Promise<void>;
  dispatch(thunk: ApplicationThunkAction ): void;
};

