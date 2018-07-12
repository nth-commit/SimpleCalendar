import * as React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Breadcrumbs, { BreadcrumbsProps } from '../../presentational/Breadcrumbs';
import { appConnect } from 'src/store/appConnect'
import { getRegionPathAboveBase, isPathLoading } from 'src/store/Regions';

export interface NavbarProps extends BreadcrumbsProps {
  isLoading: boolean;
}

const Navbar = ({ isLoading, regions }: NavbarProps) => (
  isLoading ? null :
    <div style={{ flexGrow: 1 }}>
      <AppBar position="static" color="default">
        <Toolbar>
          <Breadcrumbs regions={regions} />
        </Toolbar>
      </AppBar>
    </div>
);

export default appConnect<NavbarProps>(
  (state) => ({
    regions: getRegionPathAboveBase(state),
    isLoading: isPathLoading(state)
  })
)(Navbar);
