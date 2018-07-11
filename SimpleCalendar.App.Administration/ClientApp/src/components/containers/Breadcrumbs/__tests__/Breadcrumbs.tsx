import * as React from 'react';
import { DeepPartial } from 'redux';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import { shallow } from 'enzyme';
import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationState } from 'src/store';
import Breadcrumbs, { UnconnectedBreadcrumbs, BreadcrumbsProps } from '../';

const renderBreadcrumbs = (state: DeepPartial<ApplicationState>) => {
  const mockStore = configureStore();
  const store = mockStore(state);
  return shallow(
    <Provider store={store}>
      <Breadcrumbs />
    </Provider>
  );
};

const renderUnconnectedBreadcrumbs = (props: DeepPartial<BreadcrumbsProps> = {}) => shallow(
  <UnconnectedBreadcrumbs
    pathname={props.pathname || '/'}
    regions={(props.regions || []) as any}
    baseRegionId={props.baseRegionId || ROOT_REGION_ID} />);

describe('component: Breadcrumbs', () => {

  it('should render without throwing', () => {

    const breadcrumbs = renderBreadcrumbs({
      configuration: {
        baseRegionId: 'ROOT'
      },
      regions: {
        regionId: 'new-zealand',
        path: [
          {
            id: 'ROOT',
            value: {
              region: {
                id: 'ROOT',
                name: 'ROOT'
              }
            }
          },
          {
            id: 'new-zealand',
            value: {
              region: {
                id: 'new-zealand',
                name: 'New Zealand'
              }
            }
          }
        ]
      }
    });

    expect(breadcrumbs.find('.breadcrumbs').exists()).toBeTruthy();
  });
});