import * as React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import { appConnect } from 'src/store/ApplicationConnect'
import { getRegionPathAboveBase, isPathLoading, RegionPath } from 'src/store/Regions';
import { authActionCreators } from 'src/store/Auth';
import Breadcrumbs from '../../presentational/Breadcrumbs';
import UserMenuTrigger from '../../presentational/UserMenuTrigger';
import createRegionHrefResolver from '../../utility/RegionHrefResolver';

export interface NavbarStateProps {
  regions: RegionPath;
  isLoading: boolean;
  baseRegionId: string;
  profileImageSrc: string;
}

export interface NavbarDispatchProps {
  logout(): void;
}

const Navbar = ({ isLoading, regions, baseRegionId, profileImageSrc, logout }: NavbarStateProps & NavbarDispatchProps) => {
  if (isLoading) {
    return null;
  }

  return (
    <div style={{ flexGrow: 1 }}>
      <AppBar position="static" color="default">
        <Toolbar style={{ display: 'flex' }}>
          <div style={{ flexGrow: 1 }}>
            <Breadcrumbs regions={regions} regionHrefResolver={createRegionHrefResolver(baseRegionId)} />
          </div>
          <UserMenuTrigger profileImageSrc={profileImageSrc} logoutClicked={logout} />
        </Toolbar>
      </AppBar>
    </div>
  )
};

export default appConnect<NavbarStateProps, NavbarDispatchProps>(
  state => ({
    regions: getRegionPathAboveBase(state),
    isLoading: isPathLoading(state),
    baseRegionId: state.configuration.baseRegionId,
    profileImageSrc: state.auth.user.picture,
  }),
  dispatch => ({
    logout: () => {
      dispatch(authActionCreators.logout());
    }
  })
)(Navbar);
