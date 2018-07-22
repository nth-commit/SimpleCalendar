import * as React from 'react';
import Avatar from '@material-ui/core/Avatar';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';

export interface UserMenuTriggerProps {
  logoutClicked(): void;
  profileImageSrc: string;
}

class UserMenuTrigger extends React.Component<UserMenuTriggerProps> {

  state = {
    anchorEl: null,
  };

  handleClick = event => {
    this.setState({ anchorEl: event.currentTarget });
  };

  handleClose = () => {
    this.setState({ anchorEl: null });
  };

  handleLogOutClick = () => {
    this.handleClose();
    this.props.logoutClicked();
  }

  render() {
    const { anchorEl } = this.state;
    const { profileImageSrc } = this.props;

    return (
      <div>
        <Avatar
          style={{ cursor: 'pointer' }}
          src={profileImageSrc}
          aria-owns={anchorEl ? '' : undefined}
          aria-haspopup="true"
          onClick={this.handleClick} />
        <Menu
          id="user-menu"
          anchorEl={anchorEl || undefined}
          open={!!anchorEl}
          onClose={this.handleClose}>
            <MenuItem onClick={this.handleLogOutClick}>Logout</MenuItem>
        </Menu>
      </div>
    );
  }
}

export default UserMenuTrigger;