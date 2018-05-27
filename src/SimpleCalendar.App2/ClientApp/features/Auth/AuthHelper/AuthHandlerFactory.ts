import { CONSTANTS } from './constants';
import { IAuthHandler } from './IAuthHandler';
import { default as Auth0AuthHandler } from './Auth0AuthHandler';

export interface IAuthHandlerFactory {
    createAuthHandler(authorityName: string): IAuthHandler;
}

export default class AuthHanderFactory implements IAuthHandlerFactory {
    createAuthHandler(authorityName: string): IAuthHandler {
        switch (authorityName.toLowerCase()) {
            case CONSTANTS.AUTHORITY_NAMES.AUTH0: return new Auth0AuthHandler();
            default: throw new Error('Not implemented');
        }
    }
}
