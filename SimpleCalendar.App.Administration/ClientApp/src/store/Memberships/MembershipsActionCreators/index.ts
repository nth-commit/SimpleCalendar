import { fetchMemberships } from './fetchMemberships'
import { fetchCurrentMemberships } from './fetchCurrentMemberships'
import { createMembership } from './createMembership'
import { deleteMembership } from './deleteMembership'

export const membershipsActionCreators = {
  fetchMemberships,
  fetchCurrentMemberships,
  createMembership,
  deleteMembership
}