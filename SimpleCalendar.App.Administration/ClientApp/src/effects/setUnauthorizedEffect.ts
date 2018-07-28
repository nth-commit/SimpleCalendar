import { ApplicationEffect } from 'src/store'
import { AuthorizationStatus, authActionCreators } from 'src/store/Auth'

const setUnauthorizedEffect: ApplicationEffect = (dispatch, getState) => {
  if (getState().auth.status !== AuthorizationStatus.Indetermined) {
    return
  }

  const rolesError = getState().roles.error
  if (rolesError) {
    dispatch(authActionCreators.setAuthorizationStatus(AuthorizationStatus.Unauthorized))
  }
}

export default setUnauthorizedEffect