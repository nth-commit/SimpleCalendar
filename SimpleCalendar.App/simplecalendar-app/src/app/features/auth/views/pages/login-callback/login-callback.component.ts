import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-auth-login-callback',
  template: ''
})
export class LoginCallbackComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.authService.onLoginCallback(params['authorityName']).then(() => {
        const redirect = localStorage.getItem('login:redirect');
        this.router.navigateByUrl(redirect || '');
      });
    });
  }
}
