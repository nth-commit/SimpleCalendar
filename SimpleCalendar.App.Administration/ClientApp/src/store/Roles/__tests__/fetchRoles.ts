import { rolesActionCreators } from '../'
import { AuthorizationStatus } from 'src/store/Auth'
import configureStore from '__testutils__/configureStore'
import { fetchMock } from '__testutils__/mocks/fetch'

describe('store.roles.fetchRoles', () => {
  const { dispatch, getState } = configureStore()

  it('[SHOULD] set authorization status to unauthorized [WHEN] /regionroles returns unauthorized', async () => {
    fetchMock.mock(
      url => new URL(url).pathname === '/regionroles',
      {
        status: 403
      },
      {
        overwriteRoutes: true
      })

    await dispatch(rolesActionCreators.fetchRoles())

    const { auth } = getState()
    expect(auth.status).toEqual(AuthorizationStatus.Unauthorized)
  })
})