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

export interface ThunkActionCreators {
  [key: string]: ThunkActionCreator
}

export interface ThunkActionCreator {
  // tslint:disable-next-line:callable-types
  (...args: any[]): ThunkAction<void, IApplicationState, {}, AnyAction>
}

export type ApplicationStore = Store<IApplicationState, AnyAction> & { dispatch(thunk: ThunkAction<any, any, any, any> )};
