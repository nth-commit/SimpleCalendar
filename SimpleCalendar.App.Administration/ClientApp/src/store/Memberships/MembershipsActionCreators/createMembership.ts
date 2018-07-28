import { ApplicationThunkActionAsync } from '../../'
import { CreateMembershipBegin, CreateMembershipComplete, CreateMembershipError } from '../MembershipsActions'
import { Api, IRegionMembershipCreate } from 'src/services/Api'

let createMembershipTrackingIdCounter = 0

export function createMembership(userEmail: string, roleId: string, regionId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { auth } = getState()

    const newMembership: IRegionMembershipCreate = {
      regionId,
      userEmail,
      regionRoleId: roleId
    }

    const createMembershipTrackingId = createMembershipTrackingIdCounter++
    dispatch(new CreateMembershipBegin(newMembership, createMembershipTrackingId, new Date().getTime()))

    try {
      const membership = await new Api(auth.accessToken).createRegionMembership(newMembership)
      dispatch(new CreateMembershipComplete(createMembershipTrackingId, membership))
    } catch {
      dispatch(new CreateMembershipError(createMembershipTrackingId))
    }
  }
}