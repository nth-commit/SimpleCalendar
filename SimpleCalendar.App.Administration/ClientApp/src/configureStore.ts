import {
  createStore,
  applyMiddleware,
  compose,
  combineReducers,
  ReducersMapObject,
  DeepPartial
} from 'redux';
import thunk from 'redux-thunk';
import { routerMiddleware, connectRouter } from 'connected-react-router';
import * as StoreModule from './store';
import { History } from 'history';

export default function configureStore(history: History, initialState: DeepPartial<StoreModule.IApplicationState>) {

  const rootReducer = getRootReducer(StoreModule.reducers);

  const store = createStore(
    connectRouter(history)(rootReducer),
    initialState,
    compose(
      applyMiddleware(
        thunk,
        routerMiddleware(history)
      )
    )
  );

  // Enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require<typeof StoreModule>('./store');
      store.replaceReducer(getRootReducer(nextRootReducer.reducers));
    });
  }

  return store as StoreModule.ApplicationStore;
}

const getRootReducer = (reducers: ReducersMapObject) => combineReducers<StoreModule.IApplicationState>(reducers as any);
