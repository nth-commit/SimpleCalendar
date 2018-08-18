import { createBrowserHistory } from 'history'
import { ROOT_REGION_ID } from 'src/constants'
import configureStoreReal from 'src/configureStore'
import { ApplicationStore, ApplicationState } from 'src/store'
import { IConfigurationState, configurationActionCreators } from 'src/store/Configuration'
import { API_BASE_PATH } from './constants'
import { fetchMock } from './mocks/fetch'

const DEFAULT_CONFIGURATION: IConfigurationState = {
  regionId: ROOT_REGION_ID,
  api: API_BASE_PATH,
  auth: {
    clientId: '',
    domain: ''
  },
  host: ''
}

export default function configureStore(state?: Partial<ApplicationState>) {
  let store: ApplicationStore

  beforeEach(() => {
    const initialState: Partial<ApplicationState> = {
      configuration: DEFAULT_CONFIGURATION,
      ...(state || {})
    }

    store = configureStoreReal(createBrowserHistory(), initialState)
    store.dispatch(configurationActionCreators.update(store.getState().configuration))

    fetchMock.mock({
      matcher: '/initFetchMock',
      response: {
        status: 404
      }
    })
  })

  afterEach(() => {
    fetchMock.reset()
    fetchMock.restore()
  })

  return {
    dispatch: (...args) => (store as any).dispatch(...args),
    getState: (...args) => (store as any).getState(...args)
  } as ApplicationStore
}