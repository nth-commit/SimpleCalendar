import * as React from 'react'
import DialogTitle from '@material-ui/core/DialogTitle'
import DialogContent from '@material-ui/core/DialogContent'
import DialogActions from '@material-ui/core/DialogActions'
import { withStyles } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import { dialogRegistration } from 'src/services/DialogRegistration'
import UserAutocomplete from '../../presentational/UserAutocomplete'
import { appConnect } from 'src/store/ApplicationConnect'
import { membershipsActionCreators } from 'src/store/Memberships'
import { uiActionCreators } from 'src/store/UI'
import { IRegionRole, IRegion } from 'src/services/Api'
import areRolesLoading from 'src/selectors/areRolesLoading'
import areRegionsLoading from 'src/selectors/areRegionsLoading'
import getCurrentRegion from 'src/selectors/getCurrentRegion'
import RoleSelector from 'src/components/presentational/RoleSelector'

export interface CreateMembershipDialogOptions {
  roleId: string
}

const styles = theme => ({
  formControl: {
    marginTop: 2 * theme.spacing.unit,
  }
})

interface CreateMembershipDialogStateProps {
  isLoading: boolean
  roles: IRegionRole[] | null
  roleId: string | null
  region: IRegion | null
}

interface CreateMembershipDialogDispatchProps {
  saved(email: string, roleId: string, regionId: string): void
  cancelled(): void
}

declare type CreateMembershipDialogProps = CreateMembershipDialogStateProps & CreateMembershipDialogDispatchProps

interface UnstyledCreateMembershipDialogState {
  email: string | null
  roleId: string | null
}

class UnstyledCreateMembershipDialog extends React.PureComponent<CreateMembershipDialogProps & { classes: any }, UnstyledCreateMembershipDialogState> {
  state = {
    email: null,
    emailExisted: false,
    roleId: null
  }

  handleUserSelected = (email) => {
    this.setState({ email })
  }

  handleRoleSelected = (roleId) => {
    this.setState({ roleId })
  }

  handleSave = () => {
    const { email, roleId } = this.state
    const { region } = this.props

    if (!region) {
      throw new Error('Expected props to be initialized')
    }

    if (email && roleId) {
      this.props.saved(email, roleId, region.id)
    }
  }

  render() {
    const { isLoading, region, roles, cancelled, classes } = this.props
    const { emailExisted } = this.state

    if (isLoading) {
      return null
    }

    if (!region || !roles) {
      throw new Error('Expected props to be initialized')
    }

    return (
      <div style={{ display: 'flex', flexDirection: 'column', width: '500px', height: '500px' }}>
        <DialogTitle>Add a membership for this role</DialogTitle>
        <DialogContent style={{ flex: 1 }}>
          <div style={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
            <div style={{ flex: 1 }}>
              <div className={classes.formControl}>
                <UserAutocomplete userSelected={this.handleUserSelected} />
              </div>
              <div className={classes.formControl}>
                <RoleSelector roleSelected={this.handleRoleSelected} roles={roles} />
              </div>
            </div>
            {emailExisted && <div>Warning: Adding a new user to the system</div>}
          </div>
        </DialogContent>
        <DialogActions>
          <Button onClick={cancelled}>
            Cancel
          </Button>
          <Button color='primary' onClick={this.handleSave}>
            Save
          </Button>
        </DialogActions>
      </div>
    )
  }
}

const UnconnectedCreateMembershipDialog = withStyles(styles)(UnstyledCreateMembershipDialog)

export const CREATE_MEMBERSHIP_DIALOG_ID = 'create-membership'

dialogRegistration.register(
  CREATE_MEMBERSHIP_DIALOG_ID,
  appConnect<CreateMembershipDialogStateProps, CreateMembershipDialogDispatchProps>(
    state => {
      const isLoading = areRolesLoading(state) || areRegionsLoading(state)
      return {
        isLoading,
        roleId: (state.ui.dialogOptions as CreateMembershipDialogOptions).roleId || null,
        roles: isLoading ? null : state.roles.roles,
        region: isLoading ? null : getCurrentRegion(state)
      }
    },
    dispatch => ({
      saved: (email, roleId, regionId) => {
        dispatch(membershipsActionCreators.createMembership(email, roleId, regionId))
        dispatch(uiActionCreators.closeDialog())
      },
      cancelled: () => dispatch(uiActionCreators.closeDialog())
    })
  )(UnconnectedCreateMembershipDialog) as any)
