import { Reducer, DeepPartial } from 'redux'
import { UIActionTypes, UIAction } from './UIActions'
import { UIState } from 'src/store/UI/UIState'

const merge = (prevState: UIState, newStatePartial: DeepPartial<UIState>): UIState =>
  Object.assign({}, prevState, newStatePartial)

export const uiReducer: Reducer = (state: UIState, action: UIAction): UIState => {
  switch (action.type) {
    case UIActionTypes.OPEN_DIALOG: {
      return merge(state, {
        dialogId: action.dialogId,
        dialogOptions: action.dialogOptions
      })
    }
    case UIActionTypes.CLOSE_DIALOG: {
      return merge(state, { dialogId: null })
    }
    default:
      return state || {}
  }
}