import { fetchRegion } from './fetchRegion';
import { fetchBaseRegionParents } from './fetchBaseRegionParents';
import { setRegion } from './setRegion';
import { createMembership } from './createMembership';
import { deleteMembership } from './deleteMembership';

export const regionActionCreators = {
  fetchRegion,
  fetchBaseRegionParents,
  setRegion,
  createMembership,
  deleteMembership
};
