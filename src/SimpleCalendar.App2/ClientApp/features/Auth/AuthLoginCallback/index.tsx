import * as React from 'react';
import { Redirect } from 'react-router-dom';
import { RouteComponentProps } from 'react-router-dom';

import { default as AuthHelper } from '../AuthHelper';

export default class AuthLoginCallback extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        AuthHelper.onLoginCallback();
        return <Redirect to="/" />;
    }
}
