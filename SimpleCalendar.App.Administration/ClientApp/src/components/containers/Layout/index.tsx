import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { Auth } from '../../services/Auth';

const _Layout = ({ children, history }) => {

  const auth = new Auth();
  if (!auth.isAuthenticated() && history.location.pathname !== '/callback') {
    auth.login();
    return null;
  }

  return (
    <div>
      {children}
    </div>
  )
};

export const Layout = withRouter(_Layout as any);
