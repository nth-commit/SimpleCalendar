import { Injectable, Injector } from '@angular/core';

import { CONSTANTS } from './constants';
import { IAuthHandler } from './auth-handler.interface';
import { Auth0AuthHandler } from './auth-handlers/auth0.auth-handler';

@Injectable()
export class AuthHanderFactory {

    constructor(
        private injector: Injector
    ) { }

    createAuthHandler(authorityName: string): IAuthHandler {
        switch (authorityName.toLowerCase()) {
            case CONSTANTS.AUTHORITY_NAMES.AUTH0: return new Auth0AuthHandler();
            default: throw new Error('Not implemented');
        }
    }
}
