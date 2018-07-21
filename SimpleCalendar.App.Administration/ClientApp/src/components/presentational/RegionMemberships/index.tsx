import * as React from 'react';
import { IRegionMembership } from 'src/services/Api';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

export interface RegionMembershipsProps {
  createClicked(): void;
  memberships: IRegionMembership[];
  classes: any;
}

export interface RegionMembershipListProps {
  memberships: IRegionMembership[];
}

const RegionMembershipsList = ({ memberships }: RegionMembershipListProps) => {
  return <div>RegionMembershipsList</div>
}

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  },
  input: {
    display: 'none',
  },
});

const RegionMemberships = ({ memberships, createClicked, classes }: RegionMembershipsProps) => (
  memberships.length ?
    <RegionMembershipsList memberships={memberships} /> :
    <div style={{ height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
      No memberships!
      <Button variant="contained" color="primary" className={classes.button} onClick={createClicked}>
        Create
      </Button>
    </div>
);

export default withStyles(styles)(RegionMemberships);