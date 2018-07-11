import * as React from 'react';
import { appConnect } from 'src/store';
import { RegionPathComponent, getRegionPathAboveBase, isPathLoading, RegionPathComponentValue } from 'src/store/Regions';

export interface BreadcrumbsProps {
  pathname: string;
  regions: RegionPathComponent[];
  loading: boolean;
}

const getBreadcrumbs = (regions: RegionPathComponent[]) => {
  const result: Array<{ name: string, pathname: string }> = [];

  result.push({
    name: 'Home',
    pathname: '/'
  });

  result.push(...regions.map(r => ({
    name: (r.value as RegionPathComponentValue).region.name,
    pathname: '/' + r.id
  })));

  return result;
}

export const UnconnectedBreadcrumbs = ({ pathname, regions, loading }: BreadcrumbsProps) => {
  if (loading) {
    return null;
  }

  return (
    <div className="breadcrumbs">
      {
        getBreadcrumbs(regions)
          .map(b => b.pathname === pathname ?
            <span className="breadcrumb" key={b.name}>
              {b.name}
            </span> :
            <a className="breadcrumb" key={b.name} href={b.pathname}>
              {b.name}
            </a>
          )
          .reduce((prev, curr, i) => [prev, <span key={i}>/</span>, curr] as any)
      }
    </div>
  );
};

export default appConnect<BreadcrumbsProps>(
  (state) => ({
    pathname: state.router.location.pathname,
    regions: getRegionPathAboveBase(state),
    loading: isPathLoading(state)
  })
)(UnconnectedBreadcrumbs);
