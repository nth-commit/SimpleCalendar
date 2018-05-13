import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './views/pages/login/login.component';
import { LoginCallbackComponent } from './views/pages/login-callback/login-callback.component';

const routes: Routes = [
  {
    path: 'auth/login',
    component: LoginComponent
  },
  {
    path: 'auth/callback',
    component: LoginCallbackComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  declarations: [
    LoginComponent,
    LoginCallbackComponent
  ]
})
export class AuthRoutingModule { }
