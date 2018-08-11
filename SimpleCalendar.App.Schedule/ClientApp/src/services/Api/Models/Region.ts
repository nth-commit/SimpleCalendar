export interface IRegion {
  id: string
  name: string
  timezone: string
  permissions: {
    canAddMemberships: {
      [membershipId: string]: boolean
    },
    canCreateEvents: boolean
    canPublishEvents: boolean
  }
}

export interface IRegionMembershipCreate {
  regionId: string
  userEmail: string
  regionRoleId: string
}

export interface IRegionMembership extends IRegionMembershipCreate {
  id: string
  permissions: {
    canDelete: boolean
  }
}

export interface IRegionMembershipQuery {
  regionId?: string
  userId?: string
}

export interface IRegionRole {
  id: string
  name: string
}