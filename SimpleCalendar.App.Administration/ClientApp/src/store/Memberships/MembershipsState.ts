import { IRegionMembership, IRegionMembershipCreate } from 'src/services/Api'

export interface MembershipIdRegionLookupEntry {
  regionId: string
  timestamp: number,
  membershipIds: string[] | null
  error?: any
}

export interface MembershipIdRegionLookup {
  [regionId: string]: MembershipIdRegionLookupEntry | undefined
}

export interface MembershipDictionaryEntry {
  membership: IRegionMembership
  timestamp: number
}

export interface MembershipDictionary {
  [membershipId: string]: MembershipDictionaryEntry | undefined
}

export interface MembershipCreationDictionaryEntry {
  membership: IRegionMembershipCreate
  timestamp: number
}

export interface MembershipCreationDictionary {
  [trackingId: number]: MembershipCreationDictionaryEntry | undefined
}

export interface MembershipsState {
  membershipDictionary: MembershipDictionary
  membershipIdRegionLookup: MembershipIdRegionLookup
  membershipCreationDictionary: MembershipCreationDictionary
}