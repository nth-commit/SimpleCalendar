import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';

import { AuthService, AuthHanderFactory } from './services/auth';

@NgModule({
  imports: [
    CommonModule,
    AuthRoutingModule
  ],
  declarations: [],
  providers: [
    AuthService,
    AuthHanderFactory
  ]
})
export class AuthModule { }
