import * as React from 'react';
import { Auth } from 'src/services/Auth';
import Breadcrumbs from '../Breadcrumbs';
import { appConnect } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';

interface LayoutStateProps {
  loading: boolean;
  isAuthCallback: boolean;
}

interface LayoutDispatchProps {
  onMounted(): void;
}

export type LayoutProps = LayoutStateProps & LayoutDispatchProps;

export class UnconnectedLayout extends React.PureComponent<LayoutProps> {

  componentDidMount() {
    const auth = new Auth();
    if (!auth.isAuthenticated() && !this.props.isAuthCallback) {
      auth.login();
    } else {
      this.props.onMounted();
    }
  }

  render() {
    if (this.props.loading) {
      return <div>loading...</div>
    } else {
      return (
        <div>
          <Breadcrumbs />
          {this.props.children}
        </div>
      );
    }
  }
}

export default appConnect<LayoutStateProps, LayoutDispatchProps>(
  state => ({
    loading: !state.regions.path.length,
    isAuthCallback: state.router.location.pathname === '/callback',
  }),
  dispatch => ({
    onMounted: () => dispatch(regionActionCreators.fetchRootRegion())
  })
)(UnconnectedLayout);
