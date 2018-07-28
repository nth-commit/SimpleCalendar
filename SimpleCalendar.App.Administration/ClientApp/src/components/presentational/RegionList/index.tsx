import * as React from 'react'
import { Link } from 'react-router-dom'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemText from '@material-ui/core/ListItemText'
import { IRegion } from 'src/services/Api'
import { RegionHrefResolver } from '../../utility/RegionHrefResolver'

export interface RegionListProps {
  regions: IRegion[]
  regionHrefResolver: RegionHrefResolver
}

const RegionList = ({ regions, regionHrefResolver }: RegionListProps) => (
  <List component="nav">
    {regions.map(r => (
      <Link key={r.id} to={regionHrefResolver.resolve(r)} style={{ textDecoration: 'none' }}>
        <ListItem button={true}>
          <ListItemText primary={r.name} />
        </ListItem>
      </Link>
    ))}
  </List>
)

export default RegionList
