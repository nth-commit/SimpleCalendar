import * as React from 'react';
import { appConnect } from 'src/store';
import { IRegion } from 'src/store/Regions';

export interface BreadcrumbsProps {
  pathname: string;
  regions: IRegion[];
  baseRegionId: string;
}

export class NoRegionsError {
  public message = 'Expected regions to render breadcrumbs';
  constructor() { }
}

const getBreadcrumbs = (regions: IRegion[], baseRegionId: string) => {
  if (!regions.length) {
    // Currently the app only contains region configuration, therefore it makes sense to sit the
    // regions at the root of the app.
    throw new NoRegionsError();
  }

  const result: Array<{ name: string, pathname: string }> = [];

  result.push({
    name: 'Home',
    pathname: '/'
  });

  return result;
}

export const UnconnectedBreadcrumbs = ({ pathname, regions, baseRegionId }: BreadcrumbsProps) => {
  return (
    <div className="breadcrumbs">
      {
        getBreadcrumbs(regions, baseRegionId)
          .map(b => b.pathname === pathname ?
            <div className="breadcrumb" key={b.name}>
              {b.name}
            </div> :
            <a className="breadcrumb" key={b.name} href={b.pathname}>
              {b.name}
            </a>
          )
          .reduce((prev, curr) => [prev, <div>/</div>, curr] as any)
      }
    </div>
  );
};

export default appConnect<BreadcrumbsProps>(
  state => ({
    pathname: state.router.location.pathname,
    regions: state.regions.path,
    baseRegionId: state.configuration.baseRegionId
  })
)(UnconnectedBreadcrumbs);
