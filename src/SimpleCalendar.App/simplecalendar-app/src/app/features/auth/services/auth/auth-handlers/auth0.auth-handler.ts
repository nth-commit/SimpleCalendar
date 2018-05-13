import { Injectable } from '@angular/core';
import { UrlTree } from '@angular/router';

import { IAuthHandler } from '../auth-handler.interface';
import { IAuthConfig } from '../auth-config.interface';
import { IAuthResult } from '../auth-result.interface';
import { CONSTANTS } from '../constants';

import { getAppSettings, AppSettings } from '../../../../../app.settings';

import { auth0Helper } from '../auth0.helper';

@Injectable()
export class Auth0AuthHandler implements IAuthHandler {

  get authConfig(): IAuthConfig {
    const auth0Settings = getAppSettings().Auth.Auth0;
    return {
      clientId: auth0Settings.ClientId,
      domain: auth0Settings.Domain,
      audience: `https://${auth0Settings.Domain}/userinfo`,
      responseType: 'token id_token',
      scope: 'open_id'
    };
  }

  handleCallback(url: UrlTree): Promise<IAuthResult> {
    const auth = auth0Helper.getAuth0(
      this.authConfig,
      CONSTANTS.AUTHORITY_NAMES.AUTH0);

    return new Promise<IAuthResult>((resolve, reject) => {
      auth.parseHash((err, authResult) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          resolve(<IAuthResult>{
            bearerToken: authResult.idToken,
            expiresIn: authResult.expiresIn,
            accessToken: authResult.accessToken
          });
        } else {
          reject(err.error);
        }
      });
    });
  }

}