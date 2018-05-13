import * as auth0 from 'auth0-js';

import { IAuthConfig } from './auth-config.interface';
import { CONSTANTS } from './constants';

import { getAppSettings, isDevelopment } from '../../../../app.settings';

class Auth0Helper {
  getAuth0(config: IAuthConfig, authorityName?: string, state?: string) {
    const appSettings = getAppSettings();
    const auth0Settings = appSettings.Auth.Auth0;
    const hostSettings = appSettings.Hosts;
    const host = isDevelopment ? "http://localhost:4200" : hostSettings.App;

    let redirectUri = `${host}/auth/callback`;
    if (authorityName && authorityName !== CONSTANTS.AUTHORITY_NAMES.AUTH0) {
        redirectUri += `/${authorityName}`;
    }

    return new auth0.WebAuth({
      clientID: config.clientId,
      domain: config.domain,
      responseType: config.responseType,
      audience: config.audience,
      scope: config.scope,
      redirectUri,
      state
    });
  }
}

export const auth0Helper = new Auth0Helper();