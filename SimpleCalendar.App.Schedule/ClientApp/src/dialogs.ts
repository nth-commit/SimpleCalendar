import { DialogProps } from '@material-ui/core/Dialog'
import { dialogRegistration } from 'src/services/DialogRegistration'
import { EVENT_DETAILS_DIALOG_ID, EventDetailsDialog } from 'src/components/dialogs/EventDetailsDialog'
import { CREATE_EVENT_DIALOG_ID, CreateEventDialog } from 'src/components/dialogs/CreateEventDialog'

const registerDialog = (dialogId: string, dialog: any, dialogProps?: Partial<DialogProps>) =>
  dialogRegistration.register(dialogId, dialog, dialogProps)

registerDialog(EVENT_DETAILS_DIALOG_ID, EventDetailsDialog, {
  fullWidth: true,
    disableBackdropClick: true
})

registerDialog(CREATE_EVENT_DIALOG_ID, CreateEventDialog, {
  fullWidth: true,
  disableBackdropClick: true
})
