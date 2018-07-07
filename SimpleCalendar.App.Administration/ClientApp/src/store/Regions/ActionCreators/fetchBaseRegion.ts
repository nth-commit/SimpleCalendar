import { ApplicationThunkActionAsync } from '../../';
import { fetchRegions } from './fetchRegions';

export function fetchBaseRegions(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    await dispatch(fetchRegions(getState().configuration.baseRegionId));
  };
}
