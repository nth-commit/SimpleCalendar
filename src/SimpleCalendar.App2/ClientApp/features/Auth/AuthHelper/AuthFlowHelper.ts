import * as auth0 from 'auth0-js';

import { IAuthConfig } from './IAuthConfig';
import { default as Auth0Helper } from './Auth0Helper';

class AuthFlowHelper {
  begin(config: IAuthConfig, authorityName?: string, state?: string) {
    const auth = Auth0Helper.getAuth0(config, authorityName);
    auth.authorize();
  }
}

export default new AuthFlowHelper();
