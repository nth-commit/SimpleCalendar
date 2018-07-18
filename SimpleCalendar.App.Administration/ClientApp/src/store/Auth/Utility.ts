import { AuthState } from './';
import { SUPER_ADMINISTRATOR, ADMINISTRATOR } from 'src/services/Api';

export function hasRegionMembership(state: AuthState) {
  return state.regionMemberships && state.regionMemberships.length;
}

export function isAdministrator(state: AuthState) {
  return hasRegionMembership(state) &&
    state.regionMemberships.some(rm => rm.regionRoleId === SUPER_ADMINISTRATOR || rm.regionRoleId === ADMINISTRATOR);
} 