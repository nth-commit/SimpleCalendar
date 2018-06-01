import { CONFIGURATION } from '../../../configuration';
import { default as Auth0Helper } from './Auth0Helper';
import { CONSTANTS } from './Constants';
import { IAuthHandler } from './IAuthHandler';
import { IAuthConfig } from './IAuthConfig';
import { IAuthResult } from './IAuthResult';

export default class Auth0AuthHandler implements IAuthHandler {

	get authConfig(): IAuthConfig {
		const auth0Settings = CONFIGURATION.Auth.Auth0;
		return {
			clientId: auth0Settings.ClientId,
			domain: auth0Settings.Domain,
			audience: `https://${auth0Settings.Domain}/userinfo`,
			responseType: 'id_token',
			scope: 'openid profile'
		};
	}

	handleCallback(): Promise<IAuthResult> {
		const auth = Auth0Helper.getAuth0(
			this.authConfig,
			CONSTANTS.AUTHORITY_NAMES.AUTH0);
	  
		  return new Promise<IAuthResult>((resolve, reject) => {
			auth.parseHash({ }, (err, authResult) => {
			  if (authResult && authResult.idToken) {
				resolve(<IAuthResult>{
				  bearerToken: authResult.idToken,
				  expiresIn: authResult.expiresIn || 1 * (24 * 60 * 60 * 1000),
				  accessToken: authResult.accessToken
				});
			  } else {
				reject(err ? err.error : err);
			  }
			});
		  });
	}
}