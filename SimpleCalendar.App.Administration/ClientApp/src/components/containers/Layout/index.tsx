import * as React from 'react';
import { connect } from 'react-redux';
import { Auth } from 'src/services/Auth';
import { ApplicationState } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';
import Breadcrumbs from '../Breadcrumbs';

export interface LayoutStateProps {
  isLoaded: boolean;
  isAuthCallback: boolean;
}

export interface LayoutDispatchProps {
  onMounted(): void;
}

export type LayoutProps = LayoutStateProps & LayoutDispatchProps;

export class UnconnectedLayout extends React.PureComponent<LayoutProps> {

  private isAuthenticated: boolean;

  componentWillMount() {
    this.isAuthenticated = new Auth().isAuthenticated() || this.props.isAuthCallback;
  }

  componentDidMount() {
    if (this.isAuthenticated) {
      this.props.onMounted();
    }
  }

  render() {
    if (!this.isAuthenticated) {
      new Auth().login();
      return null;
    }

    if (!this.props.isLoaded) {
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

export default connect<LayoutStateProps, LayoutDispatchProps, LayoutProps, ApplicationState>(
  state => ({
    isAuthCallback: state.router.location.pathname === '/callback',
    isLoaded: true
  }),
  dispatch => ({
    onMounted: () => dispatch(regionActionCreators.fetchBaseRegionParents() as any)
  })
)(UnconnectedLayout) as any;
