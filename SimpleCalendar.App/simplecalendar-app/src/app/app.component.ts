import { Component, OnInit } from '@angular/core';
import { Http, RequestOptionsArgs, Headers } from '@angular/http';

import { AuthService } from './features/auth';
import { getAppSettings } from './app.settings';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(
    private http: Http,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      const appSettings = getAppSettings();

      this.http
        .get(`${appSettings.Hosts.Api}/events`, {
          headers: new Headers({
            'Authorization': `Bearer ${this.authService.getBearerToken()}`
          })
        })
        .toPromise();
    }
  }
}
