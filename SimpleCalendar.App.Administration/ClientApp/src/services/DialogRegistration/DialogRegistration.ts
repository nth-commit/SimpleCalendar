import * as React from 'react'
import { DialogProps } from '@material-ui/core/Dialog'

export class DialogIdDuplicationException { }

export class DialogNotFoundException { }

export interface DialogDefinition {
  component: typeof React.Component
  dialogProps: DialogProps
}

class DialogRegistrationService {
  private dialogs = new Map<string, DialogDefinition>()

  register(dialogId: string, component: typeof React.Component, dialogProps?: Partial<DialogProps>) {
    if (this.dialogs.has(dialogId)) {
      throw new DialogIdDuplicationException()
    }

    this.dialogs.set(dialogId, {
      component,
      dialogProps: (dialogProps || {}) as DialogProps
    })
  }

  getDialog(dialogId: string): DialogDefinition {
    const result = this.dialogs.get(dialogId)
    if (!result) {
      throw new DialogNotFoundException()
    }
    return result
  }
}

export const dialogRegistration = new DialogRegistrationService()