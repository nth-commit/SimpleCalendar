import { ApplicationThunkActionAsync } from '../../';
import { CreateMembershipBegin, CreateMembershipComplete } from '../Actions';
import { Api } from 'src/services/Api';

let createMembershipTrackingId = 0;

export function createMembership(userEmail: string, roleId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { regions, auth } = getState();

    const newMembership = {
      regionId: regions.regionId,
      userEmail,
      regionRoleId: roleId
    };

    dispatch({ ...new CreateMembershipBegin(newMembership, createMembershipTrackingId++) });

    const membership = await new Api(auth.accessToken).createRegionMembership(newMembership);

    console.log(CreateMembershipComplete, membership);
  };
}