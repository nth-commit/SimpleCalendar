import * as React from 'react'
import { Route, Switch } from 'react-router-dom'
import { withStyles } from '@material-ui/core'
import Button from '@material-ui/core/Button'
import AddIcon from '@material-ui/icons/Add'
import { Auth } from 'src/services/Auth'
import { applicationConnect, ApplicationState } from 'src/store'
import { eventsActionCreators, EventCollectionType, isFetchEventsCompletedSelector } from 'src/store/Events'
import { uiActionCreators } from 'src/store/UI'
import { regionActionCreators, regionSelectors } from 'src/store/Region'
import { authActionCreators, isAuthenticationDetermined } from 'src/store/Auth'
import { CREATE_EVENT_DIALOG_ID } from 'src/components/dialogs/CreateEventDialog'
import DialogTrigger from '../DialogTrigger'
import Navbar from '../Navbar'
import EventGroups from '../EventGroups'
import Loader from 'src/components/presentational/Loader'

interface ScheduleStateProps {
  canCreateEvents: boolean
}

interface ScheduleDispatchProps {
  createEvent(): void
}

const styles = theme => ({
  button: {
    margin: theme.spacing.unit,
  }
})

const tryAuthenticate = () => new Auth().isAuthenticated() ?
  authActionCreators.loginSuccess(localStorage.getItem('access_token') as string) :
  authActionCreators.loginSkipped()

const fetchEvents = (collection: EventCollectionType) => () => eventsActionCreators.fetchEvents(collection, true)
const isFetchEventsCompleted = (collection: EventCollectionType) => (state: ApplicationState) => isFetchEventsCompletedSelector(state, collection)

class Schedule extends React.Component<ScheduleStateProps & ScheduleDispatchProps & { classes: any }> {

  render() {
    const { canCreateEvents } = this.props

    return (
      <Loader loadAction={tryAuthenticate} isLoadedFunc={isAuthenticationDetermined}>
        <Loader loadAction={regionActionCreators.fetchRegion} isLoadedFunc={regionSelectors.isFetchRegionCompleted}>
          <Navbar />
          <div className="layout-container">
            <div className="layout-content">
              <Switch>
                <Route exact={true} path='/'>
                  <Loader loadAction={fetchEvents(EventCollectionType.TODAY)} isLoadedFunc={isFetchEventsCompleted(EventCollectionType.TODAY)}>
                    <EventGroups collection={EventCollectionType.TODAY} />
                  </Loader>
                </Route>
                <Route exact={true} path='/my-events'>
                  <Loader loadAction={fetchEvents(EventCollectionType.MY)} isLoadedFunc={isFetchEventsCompleted(EventCollectionType.MY)}>
                    <EventGroups collection={EventCollectionType.MY} />
                  </Loader>
                </Route>
                <Route><div>Not found!</div></Route>
              </Switch>
            </div>
          </div>
          {canCreateEvents && this.renderAddButton()}
          <DialogTrigger />
        </Loader>
      </Loader>
    )
  }

  private renderAddButton() {
    const { classes, createEvent } = this.props
    return (
      <div style={{ position: 'fixed', right: 0, bottom: 0 }}>
        <Button variant="fab" color="primary" aria-label="Add" className={classes.button} onClick={createEvent}>
          <AddIcon />
        </Button>
      </div>
    )
  }
}

export default applicationConnect<ScheduleStateProps, ScheduleDispatchProps>(
  state => ({
    canCreateEvents: regionSelectors.canCreateEventsInRegion(state)
  }),
  dispatch => ({
    createEvent: () => dispatch(uiActionCreators.openDialog(CREATE_EVENT_DIALOG_ID))
  }),
  undefined,
  {
    pure: false
  }
)(withStyles(styles)(Schedule))