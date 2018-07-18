export interface IRegion {
  id: string;
  name: string;
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