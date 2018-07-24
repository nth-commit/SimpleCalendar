import { ROOT_REGION_ID } from 'src/constants'
import { ApplicationThunkActionAsync } from '../../'
import { getParentRegionIds } from '../Utility'
import { fetchRegion } from './fetchRegion'

export function fetchBaseRegionParents(): ApplicationThunkActionAsync {
  return async (dispatch, getState) => {
    const { baseRegionId } = getState().configuration
    if (baseRegionId === ROOT_REGION_ID) {
      return Promise.resolve()
    }

    await Promise.all(getParentRegionIds(baseRegionId).map(id => dispatch(fetchRegion(id))))
  }
}