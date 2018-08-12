import * as React from 'react'
import { withStyles } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import AddIcon from '@material-ui/icons/Add'
import DialogTrigger from '../DialogTrigger'
import Navbar from '../Navbar'
import Effects from '../Effects'
import { appConnect } from 'src/store'
import { Auth } from 'src/services/Auth'
import { authActionCreators, AuthenticationStatus } from 'src/store/Auth'
import { regionSelectors } from 'src/store/Region'
import { uiActionCreators } from 'src/store/UI'
import { CREATE_EVENT_DIALOG_ID } from 'src/components/dialogs/CreateEventDialog'

interface LayoutStateProps {
  isLoading: boolean
  canCreateEvents: boolean
}

interface LayoutDispatchProps {
  didMount(): void
  addClicked(): void
}

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  }
})

class Layout extends React.PureComponent<LayoutStateProps & LayoutDispatchProps & { classes: any }> {

  componentDidMount() { this.props.didMount() }

  render() {
    const { isLoading, canCreateEvents, children } = this.props
    if (isLoading) {
      return null
    }

    return (
      <div>
        <Navbar />
        <div>
          {children}
        </div>
        {canCreateEvents && this.renderAddButton()}
        <DialogTrigger />
        <Effects />
      </div>
    )
  }

  private renderAddButton() {
    const { classes, addClicked } = this.props
    return (
      <div style={{ position: 'fixed', right: 0, bottom: 0 }}>
        <Button variant="fab" color="primary" aria-label="Add" className={classes.button} onClick={addClicked}>
          <AddIcon />
        </Button>
      </div>
    )
  }
}

export default appConnect<LayoutStateProps, LayoutDispatchProps>(
  state => ({
    isLoading: state.auth.status === AuthenticationStatus.Indetermined,
    canCreateEvents: regionSelectors.canCreateEventsInRegion(state)
  }),
  dispatch => ({
    didMount: () => {
      if (new Auth().isAuthenticated()) {
        dispatch(authActionCreators.loginSuccess(localStorage.getItem('access_token') as string))
      } else {
        dispatch(authActionCreators.loginSkipped())
      }
    },
    addClicked: () => dispatch(uiActionCreators.openDialog(CREATE_EVENT_DIALOG_ID))
  })
)(withStyles(styles)(Layout))