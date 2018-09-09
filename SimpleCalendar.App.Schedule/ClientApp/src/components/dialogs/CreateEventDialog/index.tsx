import * as React from 'react'
import DialogTitle from '@material-ui/core/DialogTitle'
import DialogContent from '@material-ui/core/DialogContent'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { applicationConnect } from 'src/store'
import CreateEventForm from 'src/components/presentational/CreateEventForm'
import { eventsActionCreators, IEventCreateGivenRegion } from 'src/store/Events'
import { uiActionCreators } from 'src/store/UI'

interface CreateEventDialogDispatchProps {
  saveEvent(event: IEventCreateGivenRegion)
}

declare type CreateEventDialogProps = CreateEventDialogDispatchProps

const UnconnectedCreateEventDialog = ({ saveEvent }: CreateEventDialogProps) => {
  let event: IEventCreateGivenRegion | null = null

  const onValidChange = (e: IEventCreateGivenRegion | null) => event = e

  const onSave = () => event && saveEvent(event)

  return (
    <React.Fragment>
      <DialogTitle>Create event</DialogTitle>
      <DialogContent>
        <CreateEventForm onValidChange={onValidChange} />
      </DialogContent>
      <DialogActions>
        <Button variant="raised" color="primary" onClick={onSave}>Save</Button>
      </DialogActions>
    </React.Fragment>
  )
}

export const CreateEventDialog = applicationConnect<{}, CreateEventDialogDispatchProps>(
  undefined,
  dispatch => ({
    saveEvent: event => {
      dispatch(eventsActionCreators.createEvent(event))
      dispatch(uiActionCreators.closeDialog())
    }
  })
)(UnconnectedCreateEventDialog)

export const CREATE_EVENT_DIALOG_ID = 'create-event'
