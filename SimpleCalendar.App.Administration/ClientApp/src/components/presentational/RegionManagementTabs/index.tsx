import * as React from 'react';
import { withStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import { IRegion } from 'src/services/Api';
import RegionList from '../RegionList';
import { RegionHrefResolver } from '../../utility/RegionHrefResolver';

const RegionManagementTabContainer = (props) => (
  <Typography component="div">
    {props.children}
  </Typography>
);

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
    const { childRegions, regionHrefResolver } = this.props;
    const { value } = this.state;

    if (value === 0 && childRegions.length) {
      return (
        <RegionList
          regions={childRegions}
          regionHrefResolver={regionHrefResolver} />
        );
    } else if (value === 1) {
      return (
        <RegionManagementTabContainer>
          Item Two
        </RegionManagementTabContainer>
      );
    } else {
      return (
        <RegionManagementTabContainer>
          Item Three
        </RegionManagementTabContainer>
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