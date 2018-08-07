import { IUser } from 'src/services/Api'

export enum AuthenticationStatus {
  Indetermined = 1,
  Authenticated,
  NotAuthenticated
}

export interface AuthState {
  status: AuthenticationStatus
  accessToken: string
  user: IUser | null
}