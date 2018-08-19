import * as React from 'react'
import DialogTitle from '@material-ui/core/DialogTitle'
import DialogContent from '@material-ui/core/DialogContent'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { appConnect } from 'src/store'
import { eventsActionCreators, IEventCreateGivenRegion, eventSelectors } from 'src/store/Events'
import { uiActionCreators } from 'src/store/UI'
import { IEvent } from 'src/services/Api'

interface EventDetailsDialogStateProps {
  event: IEvent
}

interface EventDetailsDialogDispatchProps {
  saveEvent(event: IEventCreateGivenRegion)
}

declare type EventDetailsDialogProps =
  EventDetailsDialogStateProps &
  EventDetailsDialogDispatchProps

const UnconnectedEventDetailsDialog = ({ event }: EventDetailsDialogProps) => {

  return (
    <React.Fragment>
      <DialogTitle>{event.name}</DialogTitle>
      <DialogContent>
        <pre>{JSON.stringify(event, undefined, '  ')}</pre>
      </DialogContent>
      <DialogActions>
        <Button variant="raised" color="primary">Close!</Button>
      </DialogActions>
    </React.Fragment>
  )
}

export interface EventDetailsDialogOptions {
  eventId: string
}

export const EventDetailsDialog = appConnect<EventDetailsDialogStateProps, EventDetailsDialogDispatchProps>(
  state => ({
    event: eventSelectors.getEventSelector(state, (state.ui.dialogOptions as EventDetailsDialogOptions).eventId)
  }),
  dispatch => ({
    saveEvent: event => {
      dispatch(eventsActionCreators.createEvent(event))
      dispatch(uiActionCreators.closeDialog())
    }
  })
)(UnconnectedEventDetailsDialog)

export const EVENT_DETAILS_DIALOG_ID = 'event-details'
