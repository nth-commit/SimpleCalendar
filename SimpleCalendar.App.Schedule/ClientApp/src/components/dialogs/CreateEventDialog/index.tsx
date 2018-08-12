import * as React from 'react'
import DialogTitle from '@material-ui/core/DialogTitle'
import DialogContent from '@material-ui/core/DialogContent'
import DialogActions from '@material-ui/core/DialogActions'
import Button from '@material-ui/core/Button'
import { dialogRegistration } from 'src/services/DialogRegistration'
import { appConnect } from 'src/store'
import CreateEventForm from 'src/components/presentational/CreateEventForm'

const UnconnectedCreateEventDialog = () => {

  return (
    <React.Fragment>
      <DialogTitle>Create event</DialogTitle>
      <DialogContent>
        <CreateEventForm onValidChange={console.log} />
      </DialogContent>
      <DialogActions>
        <Button variant="raised" color="primary">Save</Button>
      </DialogActions>
    </React.Fragment>
  )
}

const CreateEventDialog = appConnect()(UnconnectedCreateEventDialog)

export const CREATE_EVENT_DIALOG_ID = 'create-event'

dialogRegistration.register(
  CREATE_EVENT_DIALOG_ID,
  CreateEventDialog as any,
  {
    fullWidth: true,
    disableBackdropClick: true
  }
)