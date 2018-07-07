import { ROOT_REGION_ID } from 'src/constants';
import { ApplicationThunkActionAsync } from '../../';
import { fetchRegion } from './fetchRegion';

export function fetchRootRegion(): ApplicationThunkActionAsync {
  return async (dispatch) => {
    await dispatch(fetchRegion(ROOT_REGION_ID));
  };
}
