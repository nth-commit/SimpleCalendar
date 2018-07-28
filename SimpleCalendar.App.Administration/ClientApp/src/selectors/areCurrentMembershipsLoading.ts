import { ApplicationState } from 'src/store'

export default function areCurrentMembershipsLoading(state: ApplicationState): boolean {
  const { regions, memberships } = state

  const { regionId } = regions
  if (!regionId) {
    return true
  }

  const membershipRegionEntry = memberships.membershipIdRegionLookup[regionId]
  if (!membershipRegionEntry) {
    return true
  }

  const membershipIds = membershipRegionEntry.membershipIds
  if (!membershipIds) {
    return true
  }

  return membershipIds.some(m => !memberships.membershipDictionary[m])
}