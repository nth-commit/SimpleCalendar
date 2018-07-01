import {
  createStore,
  applyMiddleware,
  compose,
  combineReducers,
  Store,
  StoreEnhancerStoreCreator,
  ReducersMapObject
} from 'redux';
import thunk from 'redux-thunk';
import { routerReducer, routerMiddleware } from 'react-router-redux';
import * as StoreModule from './store';
import { History } from 'history';

declare type GenericStoreEnhancer = <S>(next: StoreEnhancerStoreCreator<S>) => StoreEnhancerStoreCreator<S>;

export default function configureStore(history: History, initialState?: StoreModule.IApplicationState) {
  // Build middleware. These are functions that can process the actions before they reach the store.
  const windowIfDefined = typeof window === 'undefined' ? null : window as any;
  // If devTools is installed, connect to it
  const devToolsExtension = windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__  as () => GenericStoreEnhancer;

  const createStoreWithMiddleware: any = compose(
    applyMiddleware(thunk, routerMiddleware(history)),
    devToolsExtension ? devToolsExtension() : <S>(next: StoreEnhancerStoreCreator<S>) => next
  )(createStore);

  // Combine all reducers and instantiate the app-wide store instance
  const allReducers = buildRootReducer(StoreModule.reducers);
  const store = createStoreWithMiddleware(allReducers, initialState) as Store<StoreModule.IApplicationState>;

  // Enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require<typeof StoreModule>('./store');
      store.replaceReducer(buildRootReducer(nextRootReducer.reducers));
    });
  }

  return store;
}

function buildRootReducer(allReducers: ReducersMapObject) {
  const reducers: any = Object.assign({}, allReducers, { routing: routerReducer });
  return combineReducers<StoreModule.IApplicationState>(reducers);
}
