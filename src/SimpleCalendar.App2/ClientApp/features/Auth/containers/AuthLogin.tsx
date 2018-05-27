import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import * as queryString from 'query-string';

export default class AuthLogin extends React.Component<RouteComponentProps<{}>, {}> {

    public render() {
        const query = queryString.parse(location.search);
        if (query.redirect) {
            localStorage.setItem('login:redirect', query.redirect);
        }

        return (
            <div>
                AuthLogin
            </div>
        );
    }
}
