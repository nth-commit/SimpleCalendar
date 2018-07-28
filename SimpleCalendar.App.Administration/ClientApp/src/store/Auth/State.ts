import { IUser } from 'src/services/Api'

export enum AuthorizationStatus {
  Indetermined = 1,
  Authorized,
  Unauthorized
}

export interface AuthState {
  status: AuthorizationStatus
  accessToken: string
  user: IUser
}