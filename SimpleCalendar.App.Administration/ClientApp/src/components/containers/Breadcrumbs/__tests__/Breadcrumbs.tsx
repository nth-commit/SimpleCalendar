import * as React from 'react';
import { shallow, mount, render } from 'enzyme';
import { ROOT_REGION_ID } from 'src/constants';
import { UnconnectedBreadcrumbs, BreadcrumbsProps, NoRegionsError } from '../';


describe('component: Breadcrumbs', () => {

  const renderBreadcrumbs = (props: BreadcrumbsProps) => shallow(
    <UnconnectedBreadcrumbs
      pathname={props.pathname}
      regions={props.regions}
      baseRegionId={props.baseRegionId} />);
  
  it('should throw NoRegionsError when rendered with no regions', () => {
    expect(() => renderBreadcrumbs({
      pathname: '/',
      baseRegionId: ROOT_REGION_ID,
      regions: []
    })).toThrowError(NoRegionsError);
  });

  it('should render without throwing', () => {
    const rendered = renderBreadcrumbs({
      pathname: '/',
      baseRegionId: ROOT_REGION_ID,
      regions: [
        {
          id: ROOT_REGION_ID,
          name: ROOT_REGION_ID
        }
      ]
    });
    
    expect(rendered.find('.breadcrumbs').exists()).toBeTruthy();
  });
});