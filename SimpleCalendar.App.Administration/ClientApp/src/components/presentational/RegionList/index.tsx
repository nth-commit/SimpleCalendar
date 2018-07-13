import * as React from 'react';
import { ROOT_REGION_ID } from 'src/constants';
import { IRegion } from 'src/services/Api';

export interface RegionListProps {
  regions: IRegion[];
  baseRegionId: string;
}

class RegionNotOfBaseError {
  public message = 'Region is not related to baseRegionId';
}

const getRegionPath = (regionId: string, baseRegionId: string): string => {
  if (baseRegionId === ROOT_REGION_ID) {
    if (regionId === ROOT_REGION_ID) {
      return '/';
    } else {
      return '/' + regionId;
    }
  }

  if (!regionId.startsWith(baseRegionId)) {
    throw new RegionNotOfBaseError();
  }

  return '/' + regionId.substring(baseRegionId.length + 1);
};

// TODO: Get working when root not base
const RegionList = ({ regions, baseRegionId }: RegionListProps) => (
  <div>
    {regions.map(r => <div key={r.id}><a href={getRegionPath(r.id, baseRegionId)}>{r.name}</a></div>)}
  </div>
);

export default RegionList;
