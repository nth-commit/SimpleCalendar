import { ApplicationState } from 'src/store'
import { IRegionMembership } from 'src/services/Api'
import getRegionIdOrThrow from 'src/selectors/getRegionIdOrThrow'

class MembershipRegionEntryNotFoundError {
  constructor(public regionId: string) { }
}

class MembershipsNotLoadedError {
  constructor(public regionId: string) { }
}

class MembershipNotLoadedError {
  constructor(public membershipId: string) { }
}

export default function getCurrentMemberships(state: ApplicationState): IRegionMembership[] {
  const { memberships } = state

  const regionId = getRegionIdOrThrow(state)
  const membershipRegionEntry = memberships.membershipIdRegionLookup[regionId]
  if (!membershipRegionEntry) {
    throw new MembershipRegionEntryNotFoundError(regionId)
  }

  const membershipIds = membershipRegionEntry.membershipIds
  if (!membershipIds) {
    throw new MembershipsNotLoadedError(regionId)
  }

  return membershipIds.map(m => {
    const membershipEntry = memberships.membershipDictionary[m]
    if (!membershipEntry) {
      throw new MembershipNotLoadedError(m)
    }
    return membershipEntry.membership
  })
}