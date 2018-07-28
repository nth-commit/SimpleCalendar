import { Reducer } from 'redux'
import { MembershipsState, MembershipIdRegionLookupEntry, MembershipIdRegionLookup, MembershipDictionaryEntry } from './MembershipsState'
import * as MembershipActions from './MembershipsActions'
import { InvokedReducer } from 'src/store/InvokedReducer'
import { IRegionMembership } from 'src/services/Api'
const { MembershipActionTypes } = MembershipActions

export class MembershipIdRegionEntryNotFound {
  constructor(public regionId: string) { }
}

const DEFAULT_MEMBERSHIPS_STATE: MembershipsState = {
  membershipDictionary: {},
  membershipIdRegionLookup: {},
  membershipCreationDictionary: {}
}

const addMembershipsToLookup = (state: MembershipsState, memberships: IRegionMembership[], timestamp: number): MembershipsState => {
  const { membershipDictionary } = state
  return {
    ...state,
    membershipDictionary: {
      ...membershipDictionary,
      ...memberships
        .map<MembershipDictionaryEntry>(membership => ({
          membership,
          timestamp
        }))
        .toObject(e => e.membership.id)
    }
  }
}

const updateMembershipRegionLookup = (state: MembershipsState, membershipIdRegionLookup: MembershipIdRegionLookup): MembershipsState =>
  ({ ...state, ...{ membershipIdRegionLookup } })

const updateEntryInMembershipRegionLookup = (state: MembershipsState, regionId: string, newPartialEntry: Partial<MembershipIdRegionLookupEntry>): MembershipsState => {
  const { membershipIdRegionLookup } = state

  const entry = membershipIdRegionLookup[regionId]
  if (!entry) {
    throw new MembershipIdRegionEntryNotFound(regionId)
  }

  return updateMembershipRegionLookup(state, {
    ...state.membershipIdRegionLookup,
    [regionId]: {
      ...entry,
      ...newPartialEntry
    }
  })
}

const fetchMembershipsBegin: InvokedReducer<MembershipsState, MembershipActions.FetchMembershipsBegin> = (state, { regionId, timestamp }) => {
  return {
    ...state,
    membershipIdRegionLookup: {
      ...state.membershipIdRegionLookup,
      [regionId]: {
        regionId,
        timestamp,
        membershipIds: null
      }
    }
  }
}

const fetchMembershipsComplete: InvokedReducer<MembershipsState, MembershipActions.FetchMembershipsComplete> = (state, { regionId, memberships }) => {
  const stateWithMembershipIdUpdate = updateEntryInMembershipRegionLookup(state, regionId, {
    membershipIds: memberships.map(m => m.id)
  })
  return addMembershipsToLookup(stateWithMembershipIdUpdate, memberships, (state.membershipIdRegionLookup[regionId] as MembershipIdRegionLookupEntry).timestamp)
}

const createMembershipBegin: InvokedReducer<MembershipsState, MembershipActions.CreateMembershipBegin> = (state, { trackingId, membership }) => ({
  ...state,
  membershipCreationDictionary: {
    ...state.membershipCreationDictionary,
    [trackingId]: {
      membership,
      timestamp: 0
    }
  }
})

const createMembershipComplete: InvokedReducer<MembershipsState, MembershipActions.CreateMembershipComplete> = (state, { trackingId, membership }) => {
  const trackingIdStr = trackingId.toString()

  const withMembershipCreationEntryRemoved = (s: MembershipsState): MembershipsState => ({
    ...s,
    membershipCreationDictionary: Object.filter(state.membershipCreationDictionary, k => k !== trackingIdStr)
  })

  const withMembershipEntryAdded = (s: MembershipsState): MembershipsState => {
    const { regionId } = membership

    const membershipIdRegionLookupEntry = s.membershipIdRegionLookup[regionId]
    if (membershipIdRegionLookupEntry && membershipIdRegionLookupEntry.membershipIds) {
      return {
        ...s,
        membershipIdRegionLookup: {
          ...s.membershipIdRegionLookup,
          [regionId]: {
            ...membershipIdRegionLookupEntry,
            membershipIds: [...membershipIdRegionLookupEntry.membershipIds, membership.id]
          }
        },
        membershipDictionary: {
          ...s.membershipDictionary,
          [membership.id]: {
            membership,
            timestamp: 0
          }
        }
      }
    }

    return s
  }

  return withMembershipEntryAdded(withMembershipCreationEntryRemoved(state))
}

const deleteMembershipBegin: InvokedReducer<MembershipsState, MembershipActions.DeleteMembershipBegin> = (state, { membership }) => {

  const withMembershipEntryRemoved = (s: MembershipsState): MembershipsState => ({
    ...s,
    membershipDictionary: Object.filter(s.membershipDictionary, k => k !== membership.id)
  })

  const withMembershipRegionEntryRemoved = (s: MembershipsState): MembershipsState => {
    const { membershipIdRegionLookup } = s
    const { regionId, id } = membership

    const membershipRegionEntry = membershipIdRegionLookup[regionId]
    if (!membershipRegionEntry) {
      return s
    }

    return {
      ...s,
      membershipIdRegionLookup: {
        ...membershipIdRegionLookup,
        [regionId]: {
          ...membershipRegionEntry,
          membershipIds: membershipRegionEntry.membershipIds ?
            membershipRegionEntry.membershipIds.filter(m => m !== id) :
            null
        }
      }
    }
  }

  return withMembershipRegionEntryRemoved(withMembershipEntryRemoved(state))
}

export const membershipsReducer: Reducer<MembershipsState, MembershipActions.MembershipAction> = (state = DEFAULT_MEMBERSHIPS_STATE, action) => {
  switch (action.type) {
    case MembershipActionTypes.FETCH_MEMBERSHIPS_BEGIN: return fetchMembershipsBegin(state, action)
    case MembershipActionTypes.FETCH_MEMBERSHIPS_COMPLETE: return fetchMembershipsComplete(state, action)
    case MembershipActionTypes.CREATE_MEMBERSHIP_BEGIN: return createMembershipBegin(state, action)
    case MembershipActionTypes.CREATE_MEMBERSHIP_COMPLETE: return createMembershipComplete(state, action)
    case MembershipActionTypes.DELETE_MEMBERSHIP_BEGIN: return deleteMembershipBegin(state, action)
    default: return state
  }
}