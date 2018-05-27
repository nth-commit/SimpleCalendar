import * as auth0 from 'auth0-js';

import { IAuthConfig } from './IAuthConfig';
import { CONSTANTS } from './constants';
import { CONFIGURATION } from '../../../configuration';

class Auth0Helper {
	getAuth0(config: IAuthConfig, authorityName?: string) {
		const auth0Settings = CONFIGURATION.Auth.Auth0;
		const hostSettings = CONFIGURATION.Hosts;
		const host = hostSettings.App;

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
		});
	}
}

export default new Auth0Helper();
