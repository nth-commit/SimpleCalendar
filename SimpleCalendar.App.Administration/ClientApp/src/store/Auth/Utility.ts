import { AuthState } from './'

export function hasRegionMembership(state: AuthState) {
  return state.regionMemberships && state.regionMemberships.length;
}

export function isAdministrator(state: AuthState) {
  return hasRegionMembership(state) && state.regionMemberships.some(rm => rm.role === 3);
} 