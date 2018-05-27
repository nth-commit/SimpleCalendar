import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import * as queryString from 'query-string';

import { default as AuthHelper } from '../AuthHelper';

export default class AuthLogin extends React.Component<RouteComponentProps<{}>, {}> {

    public render() {
        const query = queryString.parse(location.search);
        if (query.redirect) {
            localStorage.setItem('login:redirect', query.redirect);
        }

        AuthHelper.redirectToLogin();

        return (
            <div>
                Logging in...
            </div>
        );
    }
}
