import { ApplicationThunkAction } from '../'
import { OpenDialogAction, CloseDialogAction } from './Actions'

export class DialogAlreadyOpenException { }

export class DialogNotOpenException { }

function openDialog(dialogId: string, dialogOptions: any = {}): ApplicationThunkAction {
  return (dispatch, getState) => {
    if (getState().ui.dialogId) {
      throw new DialogAlreadyOpenException()
    }

    dispatch({ ...new OpenDialogAction(dialogId, dialogOptions) })
  }
}

function closeDialog(): ApplicationThunkAction {
  return (dispatch, getState) => {
    if (!getState().ui.dialogId) {
      throw new DialogNotOpenException()
    }

    dispatch({ ...new CloseDialogAction() })
  }
}

export const uiActionCreators = {
  openDialog,
  closeDialog
}
