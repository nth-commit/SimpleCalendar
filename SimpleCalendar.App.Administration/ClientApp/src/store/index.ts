export interface IApplicationState {
  [key: string]: any;
}

export const reducers = {
};

export type IAppThunkAction<TAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => void;
