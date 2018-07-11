import { ApplicationThunkActionAsync, ApplicationState } from '../../';
import { Api } from 'src/services/Api';
import { FetchRegionMembershipsBegin, FetchRegionMembershipsComplete } from '../Actions';

export default function fetchRegionMemberships(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    validateRegionMemberships(getState());

    dispatch({ ...new FetchRegionMembershipsBegin() });

    const regionMemberships = await new Api().getMyRegionMemberships();

    dispatch({ ...new FetchRegionMembershipsComplete(regionMemberships) });
  };
}

function validateRegionMemberships(state: ApplicationState) {
  const { regionMemberships, regionMembershipsLoading } = state.auth;
  if (regionMembershipsLoading || regionMemberships) {
    throw new Error('Refreshing region memberships is not supported');
  }
}
