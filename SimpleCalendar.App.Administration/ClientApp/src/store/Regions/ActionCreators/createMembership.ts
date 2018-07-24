import { ApplicationThunkActionAsync } from '../../'
import { CreateMembershipBegin, CreateMembershipComplete, CreateMembershipError } from '../Actions'
import { Api } from 'src/services/Api'

let createMembershipTrackingIdCounter = 0

export function createMembership(userEmail: string, roleId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { regions, auth } = getState()

    const newMembership = {
      regionId: regions.regionId,
      userEmail,
      regionRoleId: roleId
    }

    const createMembershipTrackingId = createMembershipTrackingIdCounter++
    dispatch({ ...new CreateMembershipBegin(newMembership, createMembershipTrackingId) })

    try {
      const membership = await new Api(auth.accessToken).createRegionMembership(newMembership)
      dispatch({ ...new CreateMembershipComplete(createMembershipTrackingId, membership) })
    } catch {
      dispatch({ ...new CreateMembershipError(createMembershipTrackingId) })
    }
  }
}