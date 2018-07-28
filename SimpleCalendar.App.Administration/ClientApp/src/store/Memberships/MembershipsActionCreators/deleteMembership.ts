import { ApplicationThunkActionAsync } from '../../'
import { DeleteMembershipBegin, DeleteMembershipComplete, DeleteMembershipError } from '../MembershipsActions'
import { Api } from 'src/services/Api'

export class MembershipNotFoundError {
  constructor(public membershipId: string) { }
}


export function deleteMembership(membershipId: string): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { auth, memberships } = getState()

    const membershipEntry = memberships.membershipDictionary[membershipId]
    if (!membershipEntry) {
      throw new MembershipNotFoundError(membershipId)
    }

    dispatch(new DeleteMembershipBegin(membershipEntry.membership))

    let error: any = null
    try {
      await new Api(auth.accessToken).deleteRegionMembersip(membershipId)
    } catch (e) {
      error = e
    }

    if (error) {
      dispatch(new DeleteMembershipError(error))
    } else {
      dispatch(new DeleteMembershipComplete(membershipId))
    }
  }
}
