import * as React from 'react';
import DialogTitle from '@material-ui/core/DialogTitle';
import { dialogRegistration } from 'src/services/DialogRegistration';

const UnconnectedTestDialog = () => <DialogTitle>Hi from test dialog!</DialogTitle>;

export const TEST_DIALOG_ID = 'test';

dialogRegistration.register(TEST_DIALOG_ID, UnconnectedTestDialog as any);
