import * as React from 'react'
import { IRegionMembership, IRegionRole } from 'src/services/Api'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemText from '@material-ui/core/ListItemText'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import MembershipMenuTrigger from '../MembershipMenuTrigger'

export interface MembershipListProps {
  deleteClicked(membershipId: string)
  memberships: IRegionMembership[]
  roles: IRegionRole[]
  regionId: string
}

export default ({ memberships, roles, regionId, deleteClicked }: MembershipListProps) => {
  const rolesById = Map.fromArray(roles, r => r.id)
  return (
    <List>
      {memberships.map(m => {
        const isInherited = m.regionId !== regionId

        let roleLabel = `${(rolesById.get(m.regionRoleId) as IRegionRole).name}`
        if (isInherited) {
          roleLabel += ' (Inherited)'
        }

        const onDeleteClicked = () => deleteClicked(m.id)

        const isVisible = !isInherited && m.permissions.canDelete
        return (
          <ListItem key={m.userEmail}>
            <ListItemText primary={m.userEmail} style={{ flex: 'initial' }} />
            <ListItemText secondary={roleLabel} />
            <ListItemIcon>
              <MembershipMenuTrigger
                isVisible={isVisible}
                deleteClicked={onDeleteClicked}
                membership={m} />
            </ListItemIcon>
          </ListItem>
        )
      })}
    </List>
  )
}