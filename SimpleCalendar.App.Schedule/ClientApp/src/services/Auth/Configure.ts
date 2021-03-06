export interface IAuthConfiguration {
  clientId: string
  domain: string
  redirectUri: string
}

let config: IAuthConfiguration

export const getConfiguration = (): IAuthConfiguration => {
  if (!config) {
    throw new Error('Auth not configured')
  }
  return config
}

export const setConfiguration = (configuration: IAuthConfiguration) => {
  config = configuration
}