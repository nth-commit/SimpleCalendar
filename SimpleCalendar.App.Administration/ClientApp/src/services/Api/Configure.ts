export interface IApiConfiguration {
  baseUri: string;
}

let config: IApiConfiguration | undefined;

export const getConfiguration = (): IApiConfiguration => {
  if (!config) {
    throw new Error('Api not configured');
  }
  return config;
};

export const setConfiguration = (configuration: IApiConfiguration) => {
  config = configuration;
};

export const clearConfiguration = () => {
  config = undefined;
}