import { CONSTANTS } from './Constants';
import { IAuthConfig } from './IAuthConfig';
import { IAuthResult } from './IAuthResult';
import { IAuthHandler } from './IAuthHandler';
import { IAuthHandlerFactory } from './AuthHandlerFactory';
import { default as AuthFlowHelper } from './AuthFlowHelper';

interface AuthLocalStorageItem {
	expiresAt: number;
	bearerToken: string;
	authResult: IAuthResult;
}

export default class AuthHelper {

	constructor(
		private authHandlerFactory: IAuthHandlerFactory
	) { }

	redirectToLogin(authorityName?: string) {
		const resolvedAuthorityName = this.getAuthorityName(authorityName);
		const authHandler = this.authHandlerFactory.createAuthHandler(resolvedAuthorityName);

		const state = `${resolvedAuthorityName}-${new Date().getTime()}`;
		localStorage.setItem('auth:state', state);

		AuthFlowHelper.begin(authHandler.authConfig, resolvedAuthorityName, state);
	}

	onLoginCallback(authorityName?: string): Promise<void> {
		const authHandler = this.authHandlerFactory.createAuthHandler(this.getAuthorityName(authorityName));

		const state = localStorage.getItem('auth:state');
		localStorage.removeItem('auth:state');

		return authHandler.handleCallback()
			.then(authResult => {
				this.setAuthLocalStorageItem({
					expiresAt: (authResult.expiresIn * 1000) + new Date().getTime(),
					bearerToken: authResult.bearerToken,
					authResult
				}, authorityName);
			});
	}

	logout(authorityName?: string) {
		localStorage.removeItem(this.getLocalStorageKey(authorityName));
		// TODO: Navigate to home
	}

	isAuthenticated(authorityName?: string): boolean {
		const authInfo = this.getAuthLocalStorageItem(authorityName);
		return authInfo ? new Date().getTime() < authInfo.expiresAt : false;
	}

	getBearerToken(authorityName?: string): string | null {
		const authInfo = this.getAuthLocalStorageItem(authorityName);
		return authInfo ? authInfo.bearerToken : null;
	}

	private setAuthLocalStorageItem(authInfo: AuthLocalStorageItem, authorityName?: string) {
		this.setLocalStorageItem(JSON.stringify(authInfo), authorityName);
	}

	private getAuthLocalStorageItem(authorityName?: string): AuthLocalStorageItem {
		const json = this.getLocalStorageItem(authorityName);
		return json ? JSON.parse(json) : null;
	}

	private setLocalStorageItem(item: any, authorityName?: string) {
		return localStorage.setItem(this.getLocalStorageKey(authorityName), item);
	}

	private getLocalStorageItem(authorityName?: string): any {
		return localStorage.getItem(this.getLocalStorageKey(authorityName));
	}

	private getLocalStorageKey(authorityName?: string): string {
		return `auth:${this.getAuthorityName(authorityName)}`;
	}

	private getAuthorityName(authorityName?: string): string {
		return authorityName || CONSTANTS.AUTHORITY_NAMES.AUTH0;
	}
}
