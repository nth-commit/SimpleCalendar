import { ApplicationThunkActionAsync } from '../../'
import { DeleteMembershipBegin, DeleteMembershipComplete } from '../Actions'
import { Api } from 'src/services/Api'

let deleteMembershipTrackingIdCounter = 0

export function deleteMembership(membershipId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { auth } = getState()

    const deleteMembershipTrackingId = deleteMembershipTrackingIdCounter++
    dispatch({ ...new DeleteMembershipBegin(membershipId, deleteMembershipTrackingId) })

    await new Api(auth.accessToken).deleteRegionMembersip(membershipId)

    dispatch({ ...new DeleteMembershipComplete(deleteMembershipTrackingId) })
  }
}