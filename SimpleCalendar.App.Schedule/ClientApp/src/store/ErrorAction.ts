import { Action } from 'redux'

export interface ErrorAction extends Action {
    error: any
}

export const isErrorAction = (action: any): action is ErrorAction => 'error' in action