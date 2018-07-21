import fetchRegionMemberships from './fetchRegionMemberships';
import setAuthorizationStatus from './setAuthorizationStatus';
import login from './login';

export const authActionCreators = {
  login,
  fetchRegionMemberships,
  setAuthorizationStatus
};