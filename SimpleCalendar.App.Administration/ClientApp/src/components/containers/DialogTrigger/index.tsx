import * as React from 'react';
import Dialog from '@material-ui/core/Dialog';
import { appConnect } from 'src/store';
import { dialogRegistration } from 'src/services/DialogRegistration';
import { uiActionCreators } from 'src/store/UI';

export interface DialogTriggerStateProps {
  dialogId: string | null;
}

export interface DialogTriggerDispatchProps {
  closed(): void;
}

class DialogTrigger extends React.Component<DialogTriggerStateProps & DialogTriggerDispatchProps> {

  render() {
    const { dialogId } = this.props;
    if (!dialogId) {
      return null;
    }

    return (
      <Dialog open={true} onClose={this.props.closed}>
        {React.createElement(dialogRegistration.getDialog(dialogId))}
      </Dialog>
    );
  }
}

export default appConnect<DialogTriggerStateProps>(
  state => ({
    dialogId: state.ui.dialogId
  }),
  dispatch => ({
    closed: () => dispatch(uiActionCreators.closeDialog())
  })
)(DialogTrigger) as any;
