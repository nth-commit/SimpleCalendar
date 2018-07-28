import * as React from 'react'
import { withStyles } from '@material-ui/core/styles'
import AppBar from '@material-ui/core/AppBar'
import Tabs from '@material-ui/core/Tabs'
import Tab from '@material-ui/core/Tab'
import { IRegion } from 'src/services/Api'
import { RegionHrefResolver } from '../../utility/RegionHrefResolver'
import RegionList from '../RegionList'
import Memberships from 'src/components/containers/Memberships'
import TabContent from 'src/components/presentational/TabContent'

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
  createMembershipClicked(): void
  createRegionClicked(): void
  region: IRegion
  childRegions: IRegion[]
  regionHrefResolver: RegionHrefResolver
  classes: any
}

class RegionManagementTabs extends React.PureComponent<RegionManagementTabsProps> {

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
    const { classes, childRegions } = this.props
    const { tab, appBarHeight } = this.state

    return (
      <div className={classes.root} style={{ height: '100%' }} ref={this.appBarContainerRef}>
        <AppBar id="tabs-container" position="static">
          <Tabs value={tab} onChange={this.setTabState}>
            <Tab value={0} label="Users" />
            {childRegions.length && <Tab className={classes.tab} value={1} label="Sub-Regions" />}
          </Tabs>
        </AppBar>
        <div style={{ height: `calc(100% - ${appBarHeight}px)` }}>
          {this.renderTab()}
        </div>
      </div>
    )
  }

  private renderTab() {
    const { region, childRegions, regionHrefResolver, createMembershipClicked, createRegionClicked } = this.props
    const { tab } = this.state

    if (tab === 0) {
      const { canAddMemberships } = region.permissions 
      return (
        <TabContent
          hasAddButton={Object.keys(canAddMemberships).some(k => canAddMemberships[k])}
          onAddClick={createMembershipClicked}>
            <Memberships />
        </TabContent>
      )
    } else {
      return (
        <TabContent
          hasAddButton={false}
          onAddClick={createRegionClicked}>
            <RegionList
              regions={childRegions}
              regionHrefResolver={regionHrefResolver} />
        </TabContent>
      )
    }
  }
}

export default withStyles(styles)(RegionManagementTabs)