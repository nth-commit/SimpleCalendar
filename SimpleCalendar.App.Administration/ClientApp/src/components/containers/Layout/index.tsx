import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { Auth } from 'src/services/Auth';
import Breadcrumbs from '../Breadcrumbs';
import { appConnect } from 'src/store';

const _Layout = (args) => {
  const { children, history } = args;

  const auth = new Auth();
  if (!auth.isAuthenticated() && history.location.pathname !== '/callback') {
    auth.login();
    return null;
  }

  return (
    <div>
      <Breadcrumbs />
      {children}
    </div>
  )
};

export const Layout = appConnect(
  state => ({ state })
)(withRouter(_Layout as any));
