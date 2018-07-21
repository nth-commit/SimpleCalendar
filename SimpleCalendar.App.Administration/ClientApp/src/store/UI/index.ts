import { Reducer, DeepPartial } from 'redux';
import { UIActionTypes, UIAction } from './Actions';

export interface UIState {
  dialogId: string | null;
}

const merge = (prevState: UIState, newStatePartial: DeepPartial<UIState>): UIState =>
  Object.assign({}, prevState, newStatePartial);

export const uiReducer: Reducer = (state: UIState, action: UIAction): UIState => {
  switch (action.type) {
    case UIActionTypes.OPEN_DIALOG: {
      return merge(state, { dialogId: action.dialogId });
    }
    case UIActionTypes.CLOSE_DIALOG: {
      return merge(state, { dialogId: null });
    }
    default:
      return state || {};
  }
}

export * from './Actions';
export * from './ActionCreators';