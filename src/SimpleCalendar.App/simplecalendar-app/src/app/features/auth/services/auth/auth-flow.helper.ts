import * as auth0 from 'auth0-js';

import { IAuthConfig } from './auth-config.interface';
import { auth0Helper } from './auth0.helper';

class AuthFlowHelper {
  begin(config: IAuthConfig, authorityName?: string, state?: string) {
    const auth = auth0Helper.getAuth0(config, authorityName, state);
    auth.authorize();
  }
}

export const authFlowHelper = new AuthFlowHelper();