import * as React from 'react';
import { DeepPartial } from 'redux';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import { mount } from 'enzyme';
import { ApplicationState } from 'src/store';
import Breadcrumbs from '../';

const renderBreadcrumbs = (state: DeepPartial<ApplicationState>) => {
  const mockStore = configureStore();
  const store = mockStore(state);
  return mount(
    <Provider store={store}>
      <Breadcrumbs />
    </Provider>
  );
};

describe('component: Breadcrumbs', () => {

  it('should render without throwing', () => {

    const breadcrumbs = renderBreadcrumbs({
      configuration: {
        baseRegionId: 'ROOT'
      },
      router: {
        location: {
          pathname: '/'
        }
      },
      regions: {
        regionId: 'ROOT',
        path: [
          {
            id: 'ROOT',
            value: {
              region: {
                id: 'ROOT',
                name: 'ROOT'
              }
            }
          }
        ]
      }
    });

    expect(breadcrumbs.find('.breadcrumbs').exists()).toBeTruthy();
  });
});