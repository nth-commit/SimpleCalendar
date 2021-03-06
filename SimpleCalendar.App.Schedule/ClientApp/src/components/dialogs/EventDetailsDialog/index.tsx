import * as React from 'react'
import DialogTitle from '@material-ui/core/DialogTitle'
import DialogContent from '@material-ui/core/DialogContent'
import IconButton from '@material-ui/core/IconButton'
import CloseIcon from '@material-ui/icons/Close'
import { applicationConnect } from 'src/store'
import { getEventSelector } from 'src/store/Events'
import { uiActionCreators } from 'src/store/UI'
import { IEvent } from 'src/services/Api'
import EventDetails from 'src/components/presentational/EventDetails'
import HorizontalRule from 'src/components/presentational/HorizontalRule'
import { EventCollectionType } from 'src/store/Events/EventsActions'

interface EventDetailsDialogStateProps {
  event: IEvent
}

interface EventDetailsDialogDispatchProps {
  close(): void
}

declare type EventDetailsDialogProps =
  EventDetailsDialogStateProps &
  EventDetailsDialogDispatchProps

const UnconnectedEventDetailsDialog = ({ event, close }: EventDetailsDialogProps) => {
  return (
    <React.Fragment>
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <IconButton style={{ marginTop: '5px' }} onClick={close}>
          <CloseIcon />
        </IconButton>
        <DialogTitle style={{ paddingLeft: '0' }}>
          {event.name}
        </DialogTitle>
      </div>
      <HorizontalRule />
      <DialogContent>
        <EventDetails event={event} />
      </DialogContent>
    </React.Fragment>
  )
}

export interface EventDetailsDialogOptions {
  collection: EventCollectionType
  eventId: string
}

export const EventDetailsDialog = applicationConnect<EventDetailsDialogStateProps, EventDetailsDialogDispatchProps>(
  state => {
    const { collection, eventId } = state.ui.dialogOptions as EventDetailsDialogOptions
    return {
      event: getEventSelector(state, collection, eventId)
    }
  },
  dispatch => ({
    close: () => dispatch(uiActionCreators.closeDialog())
  })
)(UnconnectedEventDetailsDialog)

export const EVENT_DETAILS_DIALOG_ID = 'event-details'
