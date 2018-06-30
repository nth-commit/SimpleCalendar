import './index.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';
import { createBrowserHistory } from 'history';
import configureStore from './configureStore';
import { IApplicationState } from './store';
import * as RoutesModule from './routes';
import { setConfiguration as setAuthConfiguration } from './components/services/Auth';

let routes = RoutesModule.routes;

const history = createBrowserHistory({ basename: '/' });
const initialState = (window as any).initialReduxState as IApplicationState;
const store = configureStore(history, initialState);

function renderApp() {
  fetch('/config')
    .then(response => response.json())
    .then(config => {

      setAuthConfiguration({
        clientId: 'kE2HXoVFoNsXUW1QumVEE4ruh2h6AccE'
      });

      ReactDOM.render(
        <AppContainer>
          <Provider store={store}>
            <ConnectedRouter history={history} children={routes} />
          </Provider>
        </AppContainer>,
        document.getElementById('root')
      );
    });
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
	module.hot.accept('./routes', () => {
		routes = require<typeof RoutesModule>('./routes').routes;
		renderApp();
	});
}
