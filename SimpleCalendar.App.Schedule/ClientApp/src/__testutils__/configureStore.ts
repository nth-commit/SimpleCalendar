import { DeepPartial } from 'redux'
import { createBrowserHistory } from 'history'
import { ROOT_REGION_ID } from 'src/constants'
import configureStoreReal from 'src/configureStore'
import { ApplicationStore } from 'src/store'
import { configurationActionCreators, IConfigurationState } from 'src/store/Configuration'
import { API_BASE_PATH } from './constants'
import { fetchMock } from './mocks/fetch'

const DEFAULT_CONFIGURATION: DeepPartial<IConfigurationState> = {
  regionId: ROOT_REGION_ID,
  api: API_BASE_PATH
}

export default function configureStore(configuration = DEFAULT_CONFIGURATION) {
  let store: ApplicationStore

  beforeEach(() => {
    store = configureStoreReal(createBrowserHistory(), {})
    store.dispatch(configurationActionCreators.update(configuration))

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