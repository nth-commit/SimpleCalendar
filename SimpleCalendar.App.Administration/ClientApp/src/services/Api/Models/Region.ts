export interface IRegion {
  id: string;
  name: string;
  permissions: {
    canAddMemberships: {
      [membershipId: string]: boolean;
    }
  }
}

export interface IRegionMembership {
  regionId: string;
  userId: string;
  regionRoleId: string;
  permissions: {
    canDelete: boolean;
  }
}

export interface IRegionMembershipQuery {
  regionId?: string;
  userId?: string;
}

export interface IRegionRole {
  id: string;
  name: string;
}