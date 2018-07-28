import * as React from 'react'
import { Link } from 'react-router-dom'
import Typography from '@material-ui/core/Typography'
import { RegionHrefResolver } from '../../utility/RegionHrefResolver'
import { IRegion } from 'src/services/Api'

export interface BreadcrumbsProps {
  regions: IRegion[]
  regionHrefResolver: RegionHrefResolver
}

const Breadcrumbs = ({ regions, regionHrefResolver }: BreadcrumbsProps) => (
  <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
    <Link to={regionHrefResolver.resolve()} style={{ textDecoration: 'none', color: 'initial' }}>
      <Typography variant='title' color='inherit'>
        Calendar Administration
      </Typography>
    </Link>
    <div style={{ fontSize: '0.8rem' }}>
      {
        regions.map(r => (
          <span key={r.id}>
            <span style={{ marginLeft: '0.3rem' }}>></span>
            <Link style={{ marginLeft: '0.3rem' }} to={regionHrefResolver.resolve(r)}>{r.name}</Link>
          </span>
        ))
      }
    </div>
  </div>
)

export default Breadcrumbs