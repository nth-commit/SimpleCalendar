import { ApplicationEffect } from 'src/store'
import { AuthorizationStatus, authActionCreators } from 'src/store/Auth'

const setAuthorizedEffect: ApplicationEffect = (dispatch, getState) => {
  if (getState().auth.status !== AuthorizationStatus.Indetermined) {
    return
  }

  const roles = getState().roles.roles
  if (roles) {
    dispatch(authActionCreators.setAuthorizationStatus(AuthorizationStatus.Authorized))
  }
}

export default setAuthorizedEffect