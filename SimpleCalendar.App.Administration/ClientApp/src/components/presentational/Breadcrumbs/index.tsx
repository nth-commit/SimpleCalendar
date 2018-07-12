import * as React from 'react';
import Typography from '@material-ui/core/Typography'
import { RegionPath, RegionPathComponentValue } from 'src/store/Regions';

export interface BreadcrumbsProps {
  regions: RegionPath;
}

const Breadcrumbs = ({ regions }: BreadcrumbsProps) => (
  <div style={{ display: "flex", flexDirection: "row", alignItems: "center" }}>
    <a href="/" style={{ textDecoration: "none", color: "initial" }}>
      <Typography variant="title" color="inherit">
        Calendar Administration
      </Typography>
    </a>
    <div style={{ fontSize: "0.8rem" }}>
      {
        regions
          .map(r => (r.value as RegionPathComponentValue).region)
          .map(r => (
            <span key={r.id}>
              <span style={{ marginLeft: "0.3rem" }}>></span>
              <a style={{ marginLeft: "0.3rem" }} href={r.id}>{r.name}</a>
            </span>
          ))
      }
    </div>
  </div>
);

export default Breadcrumbs;