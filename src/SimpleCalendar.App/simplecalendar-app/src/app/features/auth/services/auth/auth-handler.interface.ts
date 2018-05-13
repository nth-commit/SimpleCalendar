import { UrlTree } from '@angular/router';

import { IAuthConfig } from './auth-config.interface';
import { IAuthResult } from './auth-result.interface';

export interface IAuthHandler {
  authConfig: IAuthConfig;
  handleCallback(url: UrlTree): Promise<IAuthResult>;
}
