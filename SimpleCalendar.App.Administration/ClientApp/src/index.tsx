import './index.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import configureStore from './configureStore';
import { IApplicationState } from './store';
import * as RoutesModule from './routes';

import { IConfigurationState, configurationActionCreators } from './store/Configuration';

let routes = RoutesModule.routes;

const history = createBrowserHistory({ basename: '/' });
const initialState = (window as any).initialReduxState as IApplicationState;
const store = configureStore(history, initialState);

async function renderApp() {
  const response = await fetch('/config');
  const configuration: IConfigurationState = await response.json();

  store.dispatch(configurationActionCreators.update(configuration));

  ReactDOM.render(
    <AppContainer>
      <Provider store={store}>
        <ConnectedRouter history={history} children={routes} />
      </Provider>
    </AppContainer>,
    document.getElementById('root')
  );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
  module.hot.accept('./routes', () => {
    routes = require<typeof RoutesModule>('./routes').routes;
    renderApp();
  });
}
