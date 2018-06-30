import auth0 from 'auth0-js';

export default class Auth {
  private auth0 = new auth0.WebAuth({
    domain: 'wellingtonveganactions.au.auth0.com',
    clientID: 'kE2HXoVFoNsXUW1QumVEE4ruh2h6AccE',
    redirectUri: 'http://localhost:3000/callback',
    audience: 'https://wellingtonveganactions.au.auth0.com/userinfo',
    responseType: 'token id_token',
    scope: 'openid'
  });

  public login() {
    this.auth0.authorize();
  }
}