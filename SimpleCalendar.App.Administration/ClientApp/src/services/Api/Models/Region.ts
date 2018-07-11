export interface IRegion {
  id: string;
  name: string;
}

export enum RegionRole {
  Administrator = 1,
  User = 2
}

export interface IRegionMembership {
  regionId: string;
  userId: string;
  role: RegionRole;
}

export interface IRegionMembershipQuery {
  regionId?: string;
  userId?: string;
}