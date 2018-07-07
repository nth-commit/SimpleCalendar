import { Action, AnyAction, Store, Dispatch } from 'redux';
import { ThunkAction } from 'redux-thunk';
import { ApplicationState } from './ApplicationState';

export type ApplicationThunkAction = ThunkAction<void, ApplicationState, {}, AnyAction>;
export type ApplicationThunkActionAsync = ThunkAction<Promise<void>, ApplicationState, {}, AnyAction>;

export type DispatchThunkAction = (thunk: ApplicationThunkAction) => void;
export type DispatchThunkActionAsync = (thunkAsync: ApplicationThunkActionAsync) => Promise<void>;
export type ApplicationDispatch<A extends Action<any> = AnyAction> = Dispatch<A> & DispatchThunkActionAsync & DispatchThunkAction;

export type ApplicationStore = Store<ApplicationState, AnyAction> & {
  dispatch: ApplicationDispatch;
};

// const store = {} as ApplicationStore;
// store.dispatch