import { ApplicationThunkActionAsync } from '../../'
import { LoginSkipped } from '../Actions'

export default function loginComplete(): ApplicationThunkActionAsync {
  return async (dispatch) => {
    dispatch({ ...new LoginSkipped() })
  }
}
