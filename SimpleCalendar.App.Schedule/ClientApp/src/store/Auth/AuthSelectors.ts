import { ApplicationState } from 'src/store/ApplicationState'
import { AuthenticationStatus } from 'src/store/Auth/State'

export const isAuthenticationDetermined = (state: ApplicationState) =>
  state.auth.status !== AuthenticationStatus.Indetermined