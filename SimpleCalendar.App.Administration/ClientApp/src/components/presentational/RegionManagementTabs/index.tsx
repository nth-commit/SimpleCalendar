// tslint:disable:no-bitwise
import * as React from 'react';
import { withStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import { IRegion, IRegionMembership, RegionRole } from 'src/services/Api';
import { RegionHrefResolver } from '../../utility/RegionHrefResolver';
import RegionList from '../RegionList';
import RegionMemberships from '../RegionMemberships';

const styles = theme => ({
  root: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.paper,
  },
  tab: {
    minWidth: '150px'
  }
});

export interface RegionManagementTabsProps {
  childRegions: IRegion[];
  memberships: IRegionMembership[];
  inheritedMemberships: IRegionMembership[];
  regionHrefResolver: RegionHrefResolver;
  classes: any;
}

class RegionManagementTabs extends React.Component<RegionManagementTabsProps> {
  state = {
    value: 0,
  };

  handleChange = (event, value) => {
    this.setState({ value });
  }

  renderTab() {
    const { childRegions, memberships, inheritedMemberships, regionHrefResolver } = this.props;
    const { value } = this.state;

    if (value === 0 && childRegions.length) {
      return (
        <RegionList
          regions={childRegions}
          regionHrefResolver={regionHrefResolver} />
        );
    } else if (value === 1) {
      return (
        <RegionMemberships
          memberships={[
            ...inheritedMemberships.filter(m => m.role & RegionRole.User),
            ...memberships.filter(m => m.role & RegionRole.User)
          ]} />
      );
    } else {
      return (
        <RegionMemberships
          memberships={[
            ...inheritedMemberships.filter(m => m.role & RegionRole.Administrator),
            ...memberships.filter(m => m.role & RegionRole.Administrator)
          ]} />
      );
    }
  }

  render() {
    const { classes, childRegions } = this.props;
    const { value } = this.state;

    return (
      <div className={classes.root}>
        <AppBar position="static">
          <Tabs value={value} onChange={this.handleChange}>
            {childRegions.length && <Tab className={classes.tab} label="Sub-Regions" />}
            <Tab className={classes.tab} label="Users" />
            <Tab className={classes.tab} label="Admins" />
          </Tabs>
        </AppBar>
        {this.renderTab()}
      </div>
    );
  }
}

export default withStyles(styles)(RegionManagementTabs);