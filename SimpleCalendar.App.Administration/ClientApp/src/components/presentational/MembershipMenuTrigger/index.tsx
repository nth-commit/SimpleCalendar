import * as React from 'react';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import MoreVertIcon from '@material-ui/icons/MoreVert';
import IconButton from '@material-ui/core/IconButton';
import { IRegionMembership } from 'src/services/Api';

export interface MembershipMenuTriggerProps {
  deleteClicked(): void;
  membership: IRegionMembership;
}

class MembershipMenuTrigger extends React.Component<MembershipMenuTriggerProps> {
  state = {
    anchorEl: null,
  };

  handleClick = event => {
    this.setState({ anchorEl: event.currentTarget });
  };

  handleClose = () => {
    this.setState({ anchorEl: null });
  };

  render() {
    const { membership, deleteClicked } = this.props;
    if (!membership.id) {
      return null;
    }

    const { anchorEl } = this.state;
    const menuId = `menu-${membership.id}`;
    
    const { canDelete } = membership.permissions;
    if (!canDelete) {
      return null;
    }

    const onDeleteClicked = () => {
      this.handleClose();
      deleteClicked();
    }

    return (
      <div>
        <IconButton
          aria-label="More"
          aria-owns={anchorEl ? menuId : undefined}
          aria-haspopup="true"
          onClick={this.handleClick}>
            <MoreVertIcon />
        </IconButton>
        <Menu
          id={menuId}
          anchorEl={anchorEl || undefined}
          open={!!anchorEl}
          onClose={this.handleClose}>
            <MenuItem onClick={onDeleteClicked}>Delete</MenuItem>
        </Menu>
      </div>
    );
  }
}

export default MembershipMenuTrigger;