import { AnyAction } from 'redux';
import { IRegionsState, regionsReducer } from './Regions';

export interface IApplicationState {
  regions: IRegionsState;
}

export const reducers = {
  regions: regionsReducer
};

export type IAppThunkAction<TAction = AnyAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => void;
export type IAppThunkActionAsync<TAction = AnyAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => Promise<void>;
