import * as React from 'react';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Button from '@material-ui/core/Button';
import { dialogRegistration } from 'src/services/DialogRegistration';
import UserAutocomplete from '../../presentational/UserAutocomplete';
import { appConnect } from 'src/store/ApplicationConnect';
import { ApplicationState } from 'src/store';
import { regionActionCreators } from 'src/store/Regions';
import { uiActionCreators } from 'src/store/UI';

export interface CreateMembershipDialogOptions {
  roleId: string;
}

interface CreateMembershipDialogProps {
  saved(email: string): void;
  cancelled(): void;
}

const UnconnectedCreateMembershipDialog = ({ saved, cancelled }: CreateMembershipDialogProps) => {
  let selectedEmail = '';
  let selectedEmailExisted = true;
  
  const onUserSelected = (email: string, existed: boolean) => {
    selectedEmail = email;
    selectedEmailExisted = existed;
  };

  const save = () => saved(selectedEmail);

  return (
    <div style={{ display: 'flex', flexDirection: 'column', width: '500px', height: '500px' }}>
      <DialogTitle>Add a membership for this role</DialogTitle>
      <DialogContent style={{ flex: 1 }}>
        <div style={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
          <div style={{ flex: 1 }}>
            <UserAutocomplete userSelected={onUserSelected} />
          </div>
          {selectedEmailExisted && <div>Warning: Adding a new user to the system</div>}
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={cancelled}>
          Cancel
        </Button>
        <Button color='primary' onClick={save}>
          Save
        </Button>
      </DialogActions>
    </div>
  );
};

export const CREATE_MEMBERSHIP_DIALOG_ID = 'create-membership';

dialogRegistration.register(
  CREATE_MEMBERSHIP_DIALOG_ID,
  appConnect<ApplicationState, {}, {}, CreateMembershipDialogProps>(
    state => state,
    undefined,
    (state, { dispatch }) => ({
      saved: (email) => {
        const { roleId } = state.ui.dialogOptions as CreateMembershipDialogOptions;
        dispatch(regionActionCreators.createMembership(email, roleId));
        dispatch(uiActionCreators.closeDialog());
      },
      cancelled: () => dispatch(uiActionCreators.closeDialog())
    })
  )(UnconnectedCreateMembershipDialog) as any);
