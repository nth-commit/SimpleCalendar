import { AnyAction, Store } from 'redux';
import { connect } from 'react-redux';
import { RouterState } from 'connected-react-router';
import { ThunkAction } from 'redux-thunk';
import { IRegionState, regionsReducer } from './Regions';
import { IConfigurationState, configurationReducer } from './Configuration';

export interface IApplicationState {
  router: RouterState; // TODO: Move this into configureStore, and add a new state interface that unions with this?
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

export const appConnect = <TArgs = {}>(
  mapStateToProps?: (state: IApplicationState) => TArgs,
  mapDispatchToProps?
) => connect(mapStateToProps, mapDispatchToProps);
