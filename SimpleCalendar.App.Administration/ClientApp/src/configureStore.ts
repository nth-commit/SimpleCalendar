import {
  createStore,
  applyMiddleware,
  compose,
  combineReducers,
  ReducersMapObject,
  DeepPartial,
  Middleware
} from 'redux'
import thunk from 'redux-thunk'
import { routerMiddleware, connectRouter } from 'connected-react-router'
import * as StoreModule from './store'
import { History } from 'history'
import { ApplicationDispatch, ApplicationState } from './store'
import registerEffects from 'src/effects'

const spreadClassMiddleware: Middleware<{}, ApplicationState, ApplicationDispatch> = store => next => action => {
  return next({ ...action })
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
        routerMiddleware(history)
      )
    )
  ) as StoreModule.ApplicationStore

  registerEffects(store)

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
