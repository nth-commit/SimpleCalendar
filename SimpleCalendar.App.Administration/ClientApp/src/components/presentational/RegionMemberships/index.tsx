import * as React from 'react';
import { IRegionMembership } from 'src/services/Api';

export interface RegionMembershipsProps {
  memberships: IRegionMembership[];
}

const RegionMemberships = ({ memberships }: RegionMembershipsProps) => (
  <div>{JSON.stringify(memberships)}</div>
);

export default RegionMemberships;