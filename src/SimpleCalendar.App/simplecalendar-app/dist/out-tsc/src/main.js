"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var app_module_1 = require("./app/app.module");
var environment_1 = require("./environments/environment");
if (environment_1.environment.production) {
    core_1.enableProdMode();
}
var bootstrap = function () { return platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(app_module_1.AppModule).catch(function (err) { return console.log(err); }); };
if (environment_1.environment.production) {
    core_1.enableProdMode();
    bootstrap();
}
else {
    fetch('http://localhost:5001/config')
        .then(function (response) { return response.json(); })
        .then(function (x) {
        window['ENVIRONMENT_SETTINGS'] = x;
        bootstrap();
    });
}
//# sourceMappingURL=main.js.map