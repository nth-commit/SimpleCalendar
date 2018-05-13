export interface IAuthConfig {
    clientId: string;
    domain: string;
    responseType: string;
    audience?: string;
    scope: string;
}