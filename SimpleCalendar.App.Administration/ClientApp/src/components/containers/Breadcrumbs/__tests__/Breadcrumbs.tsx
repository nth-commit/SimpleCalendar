import * as React from 'react';
import { DeepPartial } from 'redux';
import { shallow } from 'enzyme';
import { ROOT_REGION_ID } from 'src/constants';
import { UnconnectedBreadcrumbs, BreadcrumbsProps, NoRegionsError } from '../';

describe('component: Breadcrumbs', () => {

  const renderBreadcrumbs = (props: DeepPartial<BreadcrumbsProps> = {}) => shallow(
    <UnconnectedBreadcrumbs
      pathname={props.pathname || '/'}
      regions={(props.regions || []) as any}
      baseRegionId={props.baseRegionId || ROOT_REGION_ID} />);
  
  it('should throw NoRegionsError when rendered with no regions', () => {
    expect(() => renderBreadcrumbs()).toThrowError(NoRegionsError);
  });

  it('should render without throwing', () => {
    const breadcrumbs = renderBreadcrumbs({
      regions: [
        {
          id: ROOT_REGION_ID,
          name: ROOT_REGION_ID
        }
      ]
    });

    expect(breadcrumbs.find('.breadcrumbs').exists()).toBeTruthy();
  });
});