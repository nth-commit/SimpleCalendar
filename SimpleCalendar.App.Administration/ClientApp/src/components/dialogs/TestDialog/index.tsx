import * as React from 'react';
import DialogTitle from '@material-ui/core/DialogTitle';
import { dialogRegistration } from 'src/services/DialogRegistration';

class UnconnectedTestDialog extends React.Component {
  state = {
    open: true
  };

  handleOnClose = () => {
    this.setState({
      open: false
    })
  }

  render() {
    return (
      <DialogTitle>Hi from test dialog!</DialogTitle>
    );
  }
}

export const TEST_DIALOG_ID = 'test';

dialogRegistration.register(TEST_DIALOG_ID, UnconnectedTestDialog as any);
