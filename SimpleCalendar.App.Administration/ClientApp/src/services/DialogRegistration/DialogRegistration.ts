import * as React from 'react';

export class DialogIdDuplicationException { }

export class DialogNotFoundException { }

class DialogRegistrationService {
  private dialogs = new Map<string, typeof React.Component>();

  register(dialogId: string, component: typeof React.Component) {
    if (this.dialogs.has(dialogId)) {
      throw new DialogIdDuplicationException();
    }

    this.dialogs.set(dialogId, component);
  }

  getDialog(dialogId: string): typeof React.Component {
    const result = this.dialogs.get(dialogId);
    if (!result) {
      throw new DialogNotFoundException();
    }
    return result;
  }
}

export const dialogRegistration = new DialogRegistrationService();
