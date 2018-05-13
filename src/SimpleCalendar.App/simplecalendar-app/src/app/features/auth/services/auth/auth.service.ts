import { Injectable, Injector } from '@angular/core';
import { Router, UrlTree } from '@angular/router';

import { CONSTANTS } from './constants';
import { AuthHanderFactory } from './auth-handler.factory';
import { IAuthConfig } from './auth-config.interface';
import { IAuthResult } from './auth-result.interface';
import { IAuthHandler } from './auth-handler.interface';
import { authFlowHelper } from './auth-flow.helper';

interface AuthLocalStorageItem {
  expiresAt: number;
  bearerToken: string,
  authResult: IAuthResult
}

@Injectable()
export class AuthService {

  constructor(
    private authHanderFactory: AuthHanderFactory,
    private injector: Injector,
    private router: Router
  ) { }

  redirectToLogin(authorityName?: string) {
    let resolvedAuthorityName = this.getAuthorityName(authorityName);
    let authHandler = this.authHanderFactory.createAuthHandler(resolvedAuthorityName);

    let state = `${resolvedAuthorityName}-${new Date().getTime()}`;
    localStorage.setItem('auth:state', state);

    authFlowHelper.begin(authHandler.authConfig, resolvedAuthorityName, state);
  }

  onLoginCallback(authorityName?: string): Promise<void> {
    let authHandler = this.authHanderFactory.createAuthHandler(this.getAuthorityName(authorityName));

    let router = this.injector.get(Router);
    let url = router.parseUrl(router.url);

    return authHandler.handleCallback(url)
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
    this.router.navigate(['']);
  }

  isAuthenticated(authorityName?: string): boolean {
    let authInfo = this.getAuthLocalStorageItem(authorityName);
    return authInfo ? new Date().getTime() < authInfo.expiresAt : false;
  }

  getBearerToken(authorityName?: string): string {
    let authInfo = this.getAuthLocalStorageItem(authorityName);
    return authInfo ? authInfo.bearerToken : null;
  }

  private setAuthLocalStorageItem(authInfo: AuthLocalStorageItem, authorityName?: string) {
    this.setLocalStorageItem(JSON.stringify(authInfo), authorityName);
  }

  private getAuthLocalStorageItem(authorityName?: string): AuthLocalStorageItem {
    let json = this.getLocalStorageItem(authorityName);
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
