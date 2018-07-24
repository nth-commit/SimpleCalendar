import * as React from 'react'
import { withStyles } from '@material-ui/core/styles'
import AppBar from '@material-ui/core/AppBar'
import Tabs from '@material-ui/core/Tabs'
import Tab from '@material-ui/core/Tab'
import { IRegion, IRegionMembership, IRegionRole } from 'src/services/Api'
import { RegionHrefResolver } from '../../utility/RegionHrefResolver'
import RegionList from '../RegionList'
import RegionMemberships, { Memberships } from '../RegionMemberships'

const styles = theme => ({
  root: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.paper,
  },
  tab: {
    minWidth: '150px'
  }
})

export interface RegionManagementTabsProps {
  createMembershipClicked(roleId: string): void
  deleteMembershipClicked(membershipId: string): void
  roles: IRegionRole[]
  childRegions: IRegion[]
  memberships: IRegionMembership[]
  inheritedMemberships: IRegionMembership[]
  regionHrefResolver: RegionHrefResolver
  regionId: string
  classes: any
}

class RegionManagementTabs extends React.Component<RegionManagementTabsProps> {

  private appBarElement: HTMLElement | null = null
  private appBarContainerRef = (e) =>  {
    if (e) {
      this.appBarElement = e.querySelector('#tabs-container')
      this.setAppBarHeightState()
    }
  }

  state = {
    tab: 0,
    appBarHeight: 0
  }

  componentWillMount() {
    if (!this.props.childRegions.length && this.state.tab === 0) {
      this.setState({ tab: 1 })
    }
  }

  setAppBarHeightState = () => {
    if (this.appBarElement) {
      this.setState({
        appBarHeight: this.appBarElement.clientHeight
      })
    }
  }

  setTabState = (event, tab) => {
    this.setState({ tab })
  }

  render() {
    const { classes, childRegions, roles } = this.props
    const { tab, appBarHeight } = this.state

    return (
      <div className={classes.root} style={{ height: '100%' }} ref={this.appBarContainerRef}>
        <AppBar id="tabs-container" position="static">
          <Tabs value={tab} onChange={this.setTabState}>
            {childRegions.length && <Tab className={classes.tab} value={0} label="Sub-Regions" />}
            {roles.map((rr, i) => <Tab key={rr.id} className={classes.tab} value={i + 1} label={rr.name} /> )}
            <Tab value={roles.length + 1} label="Users (New!)" />
          </Tabs>
        </AppBar>
        <div style={{ height: `calc(100% - ${appBarHeight}px)` }}>
          {this.renderTab()}
        </div>
      </div>
    )
  }

  private renderTab() {
    const { childRegions, regionHrefResolver, createMembershipClicked, deleteMembershipClicked, roles, memberships, inheritedMemberships, regionId } = this.props
    const { tab } = this.state

    if (tab === 0) {
      return (
        <RegionList
          regions={childRegions}
          regionHrefResolver={regionHrefResolver} />
      )
    } else if (tab === roles.length + 1) {
      const createClicked = () => createMembershipClicked('')
      return (
        <Memberships
          createClicked={createClicked}
          deleteClicked={deleteMembershipClicked}
          regionId={regionId}
          memberships={[...memberships, ...inheritedMemberships]}
          roles={roles} />
      )
    } else {
      const regionRoleIndex = tab - 1
      const regionRoleId = this.getRegionRoleId(regionRoleIndex)
      const createClicked = () => createMembershipClicked(regionRoleId)
      return (
        <RegionMemberships
          memberships={this.getMemberships(tab - 1)}
          createClicked={createClicked} />
      )
    }
  }

  private getMemberships(regionRoleIndex: number, includeInherited = true) {
    const { memberships, inheritedMemberships, roles } = this.props
    const regionRoleId = roles[regionRoleIndex].id
    return [
      ...(includeInherited ? inheritedMemberships.filter(m => m.regionRoleId === regionRoleId) : []),
      ...memberships.filter(m => m.regionRoleId === regionRoleId)
    ]
  }

  private getRegionRoleId(regionRoleIndex: number): string {
    const { roles } = this.props
    return roles[regionRoleIndex].id
  }
}

export default withStyles(styles)(RegionManagementTabs)