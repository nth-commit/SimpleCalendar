import * as React from 'react';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { IRegion } from 'src/services/Api';
import { RegionHrefResolver } from '../../utility/RegionHrefResolver';

export interface RegionListProps {
  regions: IRegion[];
  regionHrefResolver: RegionHrefResolver;
}

const RegionList = ({ regions, regionHrefResolver }: RegionListProps) => (
  <List component="nav">
    {regions.map(r => (
      <a key={r.id} href={regionHrefResolver.resolve(r)} style={{ textDecoration: 'none' }}>
        <ListItem button={true}>
          <ListItemText primary={r.name} />
        </ListItem>
      </a>
    ))}
  </List>
);

export default RegionList;
