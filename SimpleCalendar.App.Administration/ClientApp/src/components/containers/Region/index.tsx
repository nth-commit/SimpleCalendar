import * as React from 'react'
import { appConnect } from 'src/store'
import { IRegion } from 'src/services/Api'
import { uiActionCreators } from 'src/store/UI'
import { CREATE_MEMBERSHIP_DIALOG_ID } from 'src/components/dialogs/CreateMembershipDialog'
import RegionManagementTabs from '../../presentational/RegionManagementTabs'
import createRegionHrefResolver from '../../utility/RegionHrefResolver'
import areRegionsLoading from 'src/selectors/areRegionsLoading'
import getCurrentRegionEntry from 'src/selectors/getCurrentRegionEntry'

interface RegionStateProps {
  isLoading: boolean
  region: IRegion | null
  childRegions: IRegion[] | null
  baseRegionId: string
}

interface RegionDispatchProps {
  createMembershipClicked(): void
  createRegionClicked(): void
}

export class UnconnectedRegion extends React.PureComponent<RegionStateProps & RegionDispatchProps> {

  render() {
    if (this.props.isLoading) {
      return null
    }

    const { region, childRegions, baseRegionId, createMembershipClicked, createRegionClicked } = this.props

    if (!region || !childRegions) {
      throw new Error('Expected props to be initialized')
    }

    return (
      <div style={{ height: '100%' }}>
        <RegionManagementTabs
          createMembershipClicked={createMembershipClicked}
          createRegionClicked={createRegionClicked}
          region={region}
          childRegions={childRegions}
          regionHrefResolver={createRegionHrefResolver(baseRegionId)} />
      </div>
    )
  }
}

export default appConnect<RegionStateProps, RegionDispatchProps>(
  state => {
    const isLoading = areRegionsLoading(state)
    const regionEntry = isLoading ? null : getCurrentRegionEntry(state)

    return {
      isLoading,
      region: regionEntry === null ? null : regionEntry.region,
      childRegions: regionEntry === null ? null : regionEntry.childRegions,
      baseRegionId: state.configuration.baseRegionId
    }
  },
  dispatch => ({
    createMembershipClicked: () => dispatch(uiActionCreators.openDialog(CREATE_MEMBERSHIP_DIALOG_ID)),
    createRegionClicked: () => alert('NOT AVAILABLE')
  })
)(UnconnectedRegion)
