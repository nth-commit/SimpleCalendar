import * as React from 'react'
import { IRegionMembership, IRegionRole } from 'src/services/Api'
import { withStyles } from '@material-ui/core/styles'
import Button from '@material-ui/core/Button'
import MembershipList from 'src/components/presentational/MembershipList'
import { appConnect } from 'src/store'
import { membershipsActionCreators } from 'src/store/Memberships'
import { uiActionCreators } from 'src/store/UI'
import { CREATE_MEMBERSHIP_DIALOG_ID } from 'src/components/dialogs/CreateMembershipDialog'
import getCurrentMemberships from 'src/selectors/getCurrentMemberships'
import areCurrentMembershipsLoading from 'src/selectors/areCurrentMembershipsLoading'
import areRolesLoading from 'src/selectors/areRolesLoading'
import areRegionsLoading from 'src/selectors/areRegionsLoading'

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  },
  input: {
    display: 'none',
  },
})

interface MembershipsStateProps {
  isLoading: boolean
  memberships: IRegionMembership[] | null
  roles: IRegionRole[] | null
  regionId: string | null
}

interface MembershipsDispatchProps {
  loadMemberships(): void
  createClicked(): void
  deleteClicked(membershipId: string)
}

declare type MembershipsProps = MembershipsStateProps & MembershipsDispatchProps & { classes: any }

class MembershipsUnstyled extends React.Component<MembershipsProps> {

  componentDidMount() {
    this.props.loadMemberships()
  }
  
  render() {
    const { isLoading, memberships, roles, regionId, createClicked, deleteClicked } = this.props

    if (isLoading) {
      return null
    }

    if (!regionId || !memberships || !roles) {
      throw new Error('Expected props to be available after loading')
    }

    if (memberships.length) {
      return <MembershipList
        deleteClicked={deleteClicked}
        memberships={memberships}
        roles={roles}
        regionId={regionId} />
    } else {
      return (
        <div style={{ height: '100%', display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
          No memberships!
          <Button variant="contained" color="primary" className={this.props.classes.button} onClick={createClicked}>
            Create
          </Button>
        </div>
      )
    }
  }
}

const MembershipsUnconnected = withStyles(styles)(MembershipsUnstyled)

export default appConnect<MembershipsStateProps, MembershipsDispatchProps>(
  state => {
    const isLoading =
      areCurrentMembershipsLoading(state) || 
      areRolesLoading(state) ||
      areRegionsLoading(state)

    return {
      isLoading,
      memberships: isLoading ? null : getCurrentMemberships(state),
      roles: isLoading ? null : state.roles.roles,
      regionId: state.regions.regionId
    }
  },
  dispatch => ({
    loadMemberships: () => dispatch(membershipsActionCreators.fetchCurrentMemberships()),
    createClicked: () => dispatch(uiActionCreators.openDialog(CREATE_MEMBERSHIP_DIALOG_ID)),
    deleteClicked: membershipId => dispatch(membershipsActionCreators.deleteMembership(membershipId))
  })
)(MembershipsUnconnected)