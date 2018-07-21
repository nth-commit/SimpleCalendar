import * as React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import { appConnect } from 'src/store/ApplicationConnect'
import { getRegionPathAboveBase, isPathLoading, RegionPath } from 'src/store/Regions';
import Breadcrumbs from '../../presentational/Breadcrumbs';
import createRegionHrefResolver from '../../utility/RegionHrefResolver';

export interface NavbarProps {
  regions: RegionPath;
  isLoading: boolean;
  baseRegionId: string;
}

const Navbar = ({ isLoading, regions, baseRegionId }: NavbarProps) => (
  isLoading ? null :
    <div style={{ flexGrow: 1 }}>
      <AppBar position="static" color="default">
        <Toolbar>
          <Breadcrumbs regions={regions} regionHrefResolver={createRegionHrefResolver(baseRegionId)} />
        </Toolbar>
      </AppBar>
    </div>
);

export default appConnect<NavbarProps>(
  (state) => ({
    regions: getRegionPathAboveBase(state),
    isLoading: isPathLoading(state),
    baseRegionId: state.configuration.baseRegionId
  })
)(Navbar);
