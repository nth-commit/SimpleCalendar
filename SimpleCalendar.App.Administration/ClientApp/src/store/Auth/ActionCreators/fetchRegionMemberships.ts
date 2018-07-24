import { ApplicationThunkActionAsync, ApplicationState } from '../../'
import { Api } from 'src/services/Api'
import { FetchRegionMembershipsBegin, FetchRegionMembershipsComplete } from '../Actions'

export default function fetchRegionMemberships(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    if (hasRegionMemberships(getState())) {
      return
    }

    dispatch({ ...new FetchRegionMembershipsBegin() })

    const regionMemberships = await new Api(getState().auth.accessToken).getMyRegionMemberships()

    dispatch({ ...new FetchRegionMembershipsComplete(regionMemberships) })
  }
}

function hasRegionMemberships(state: ApplicationState) {
  const { regionMemberships, regionMembershipsLoading } = state.auth
  return regionMembershipsLoading || regionMemberships
}
