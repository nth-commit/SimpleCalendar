import {
  createStore,
  applyMiddleware,
  compose,
  combineReducers,
  ReducersMapObject,
  DeepPartial,
  Middleware,
  Action
} from 'redux'
import thunk from 'redux-thunk'
import { routerMiddleware, connectRouter } from 'connected-react-router'
import * as StoreModule from './store'
import { History } from 'history'
import { ApplicationDispatch, ApplicationState, ErrorAction } from './store'

type ApplicationMiddleware = Middleware<{}, ApplicationState, ApplicationDispatch>

const spreadClassMiddleware: ApplicationMiddleware = store => next => action => {
  return next({ ...action })
}

const isErrorAction = (action: Action): action is ErrorAction => 'error' in action

const errorHandlingMiddleware: ApplicationMiddleware = () => next => action => {
  if (isErrorAction(action)) {
    console.log(action.error)
  }

  return next(action)
}

export default function configureStore(history: History, initialState: DeepPartial<StoreModule.ApplicationState>) {

  const rootReducer = getRootReducer(StoreModule.reducers)

  const store = createStore(
    connectRouter(history)(rootReducer),
    initialState,
    compose(
      applyMiddleware(
        thunk,
        spreadClassMiddleware,
        errorHandlingMiddleware,
        routerMiddleware(history)
      )
    )
  ) as StoreModule.ApplicationStore

  // Enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require<typeof StoreModule>('./store')
      store.replaceReducer(getRootReducer(nextRootReducer.reducers))
    })
  }

  return store
}

const getRootReducer = (reducers: ReducersMapObject) => combineReducers<StoreModule.ApplicationState>(reducers as any)
