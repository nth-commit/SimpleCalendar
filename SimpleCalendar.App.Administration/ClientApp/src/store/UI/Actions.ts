import { Action } from 'redux';

export enum UIActionTypes {
  OPEN_DIALOG = '[UI] OPEN_DIALOG',
  CLOSE_DIALOG = '[UI] CLOSE_DIALOG'
}

export class OpenDialogAction implements Action {
  readonly type = UIActionTypes.OPEN_DIALOG;
  constructor(public dialogId: string) { }
}

export class CloseDialogAction implements Action {
  readonly type = UIActionTypes.CLOSE_DIALOG;
}

export type UIAction =
  OpenDialogAction |
  CloseDialogAction;
