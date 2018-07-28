import { Action, AnyAction } from 'redux'
export type InvokedReducer<S = any, A extends Action = AnyAction> = (state: S, action: A) => S