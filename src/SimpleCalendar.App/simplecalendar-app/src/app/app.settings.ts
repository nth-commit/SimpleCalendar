export interface AppAuthSettings {
  ClientId: string;
  Domain: string;
}

export interface AppSettings {
  Auth: {
    Auth0: AppAuthSettings
  },
  Hosts: {
    Api: string;
    App: string;
  }
}

export const getAppSettings = (): AppSettings => window['ENVIRONMENT_SETTINGS'];
