export type Environment = 'development' | 'release';
export const environment: Environment = (window as any).SIMPLE_CALENDAR_ENVIRONMENT;

export interface IAuthConfiguration {
    ClientId: string;
    Domain: string;
}

export interface IConfiguration {
    Auth: {
        Auth0: IAuthConfiguration
    };
    Hosts: {
        Api: string;
        App: string;
        App2: string;
    };
}

export const CONFIGURATION = JSON.parse((window as any).SIMPLE_CALENDAR_CONFIGURATION);
