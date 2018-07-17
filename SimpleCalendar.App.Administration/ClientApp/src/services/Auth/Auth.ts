import auth0 from 'auth0-js';
import { getConfiguration } from './Configure';

export class Auth {

  private auth0: any;

  constructor() {
    const configuration = getConfiguration();

    this.auth0 = new auth0.WebAuth({
      domain: configuration.domain,
      clientID: configuration.clientId,
      redirectUri: configuration.redirectUri,
      audience: 'wellingtonveganactions',
      responseType: 'token id_token',
      scope: 'openid profile email'
    });
  }

  public login() {
    this.auth0.authorize();
  }

  public handleAuthentication() {
    return new Promise((resolve, reject) => this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        this.setSession(authResult);
        resolve();
      } else {
        reject();
      }
    }));
  }

  public setSession(authResult) {
    // Set the time that the Access Token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', expiresAt);
  }

  public logout() {
    // Clear Access Token and ID Token from local storage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    // navigate to the home route
    window.location.assign('/');
  }

  public isAuthenticated() {
    // Check whether the current time is past the 
    // Access Token's expiry time
    const expiresAt = parseInt(localStorage.getItem('expires_at') || '0', 10);
    return new Date().getTime() < expiresAt;
  }
}