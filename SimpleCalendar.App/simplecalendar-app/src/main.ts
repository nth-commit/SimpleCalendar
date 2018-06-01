import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

const bootstrap = () => platformBrowserDynamic().bootstrapModule(AppModule).catch(err => console.log(err));

if (environment.production) {
  enableProdMode();
  bootstrap();
} else {
  fetch('http://localhost:5001/config')
    .then(response => response.json())
    .then(x => {
      window['ENVIRONMENT_SETTINGS'] = x;
      bootstrap();
    });
}
