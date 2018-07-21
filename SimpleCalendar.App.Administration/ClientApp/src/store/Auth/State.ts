import { IRegionMembership, IUser } from 'src/services/Api';

export enum AuthorizationStatus {
  Indetermined = 1,
  Authorized,
  Unauthorized
}

export interface AuthState {
  regionMembershipsLoading: boolean;
  regionMemberships: IRegionMembership[];
  status: AuthorizationStatus;
  accessToken: string;
  user: IUser;
}