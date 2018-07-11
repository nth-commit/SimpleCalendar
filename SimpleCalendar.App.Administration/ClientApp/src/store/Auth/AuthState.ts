import { IRegionMembership } from 'src/services/Api';

export interface AuthState {
  regionMembershipsLoading: boolean;
  regionMemberships: IRegionMembership[];
}