import { IAuthConfig } from './IAuthConfig';
import { IAuthResult } from './IAuthResult';

export interface IAuthHandler {
  authConfig: IAuthConfig;
  handleCallback(): Promise<IAuthResult>;
}
