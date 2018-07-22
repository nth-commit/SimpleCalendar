import * as React from 'react';
import { IRegionMembership, IRegionRole } from 'src/services/Api';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import MembershipMenuTrigger from '../MembershipMenuTrigger';

export interface RegionMembershipsProps {
  createClicked(): void;
  memberships: IRegionMembership[];
  classes: any;
}

export interface RegionMembershipsListProps {
  memberships: IRegionMembership[];
}

const RegionMembershipsList = ({ memberships }: RegionMembershipsListProps) => {
  return <pre>{JSON.stringify(memberships, undefined, '  ')}</pre>
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

export interface MembershipListProps {
  deleteClicked(membershipId: string);
  memberships: IRegionMembership[];
  roles: IRegionRole[];
  regionId: string;
}

const MembershipList = ({ memberships, roles, regionId, deleteClicked }: MembershipListProps) => {
  const rolesById = Map.fromArray(roles, r => r.id);
  return (
    <List>
      {memberships.map(m => {
        const isInherited = m.regionId !== regionId;

        let roleLabel = `${(rolesById.get(m.regionRoleId) as IRegionRole).name}`;
        if (isInherited) {
          roleLabel += ' (Inherited)'
        }

        const onDeleteClicked = () => deleteClicked(m.id);

        return (
          <ListItem key={m.userEmail}>
            <ListItemText primary={m.userEmail} style={{ flex: 'initial' }} />
            <ListItemText secondary={roleLabel} />
            {!isInherited && (
              <ListItemIcon>
                <MembershipMenuTrigger
                  deleteClicked={onDeleteClicked}
                  membership={m} />
              </ListItemIcon>
            )}
          </ListItem>
        );
      })}
    </List>
  );
}

export interface MembershipsProps {
  createClicked(): void;
  deleteClicked(membershipId: string);
  memberships: IRegionMembership[];
  roles: IRegionRole[];
  regionId: string;
  classes: any;
}

const MembershipsUnstyled = ({ deleteClicked, memberships, createClicked, roles, regionId, classes }: MembershipsProps) => (
  memberships.length ?
    <MembershipList
      deleteClicked={deleteClicked}
      memberships={memberships}
      roles={roles}
      regionId={regionId} /> :
    <div style={{ height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
      No memberships!
      <Button variant="contained" color="primary" className={classes.button} onClick={createClicked}>
        Create
      </Button>
    </div>
);


export const Memberships = withStyles(styles)(MembershipsUnstyled);