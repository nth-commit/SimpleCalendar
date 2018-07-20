import * as React from 'react';
import { withStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import { IRegion, IRegionMembership, IRegionRole } from 'src/services/Api';
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
  roles: IRegionRole[];
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

  componentWillMount() {
    if (!this.props.childRegions.length && this.state.value === 0) {
      this.setState({ value: 1 });
    }
  }

  handleChange = (event, value) => {
    this.setState({ value });
  }

  render() {
    const { classes, childRegions, roles } = this.props;
    const { value } = this.state;

    return (
      <div className={classes.root}>
        <AppBar position="static">
          <Tabs value={value} onChange={this.handleChange}>
            {childRegions.length && <Tab className={classes.tab} value={0} label="Sub-Regions" />}
            {roles.map((rr, i) => <Tab key={rr.id} className={classes.tab} value={i + 1} label={rr.name} /> )}
          </Tabs>
        </AppBar>
        {this.renderTab()}
      </div>
    );
  }

  private renderTab() {
    const { childRegions, regionHrefResolver } = this.props;
    const { value } = this.state;

    if (value === 0) {
      return (
        <RegionList
          regions={childRegions}
          regionHrefResolver={regionHrefResolver} />
        );
    } else {
      return (
        <RegionMemberships memberships={this.getMemberships(value - 1)} />
      );
    }
  }

  private getMemberships(regionRoleIndex: number, includeInherited = true) {
    const { memberships, inheritedMemberships, roles } = this.props;
    const regionRoleId = roles[regionRoleIndex].id
    return [
      ...(includeInherited ? inheritedMemberships.filter(m => m.regionRoleId === regionRoleId) : []),
      ...memberships.filter(m => m.regionRoleId === regionRoleId)
    ]
  }
}

export default withStyles(styles)(RegionManagementTabs);