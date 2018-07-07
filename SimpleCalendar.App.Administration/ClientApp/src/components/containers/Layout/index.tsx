import * as React from 'react';
import { Auth } from 'src/services/Auth';
import { appConnect } from 'src/store';
import Breadcrumbs from '../Breadcrumbs';

export interface LayoutProps {
  isAuthCallback: boolean;
}

export class UnconnectedLayout extends React.PureComponent<LayoutProps> {

  private isAuthenticated: boolean;

  componentDidMount() {
    this.isAuthenticated = !(new Auth().isAuthenticated()) && !this.props.isAuthCallback;
  }

  render() {
    if (this.isAuthenticated) {
      new Auth().login();
      return null;
    }

    return (
      <div>
        <Breadcrumbs />
        {this.props.children}
      </div>
    );
  }
}

export default appConnect<LayoutProps>(
  state => ({
    isAuthCallback: state.router.location.pathname === '/callback',
  })
)(UnconnectedLayout) as any;
