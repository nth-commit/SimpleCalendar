import { ApplicationEffect } from 'src/store'
import { ROOT_REGION_ID } from 'src/constants'
import { regionActionCreators } from 'src/store/Regions'
import { AuthorizationStatus } from 'src/store/Auth'

let currentPathname: string | null = null
let currentBaseRegionId: string | null = null

const stripTrailingSlash = (str: string) => str.endsWith('/') ? str.substring(0, str.length - 1) : str
const stripLeadingSlash = (str: string) => str.startsWith('/') ? str.substring(1) : str

const deriveRegionId = (pathname: string, baseRegionId: string): string => {
  const regionId = baseRegionId === ROOT_REGION_ID ?
    (pathname === '/' ? ROOT_REGION_ID : pathname) :
    baseRegionId + pathname
  return stripLeadingSlash(stripTrailingSlash(regionId))
}

const setRegionEffect: ApplicationEffect = (dispatch, getState) => {
  const { configuration, router, auth } = getState()

  const isAuthorized = auth.status === AuthorizationStatus.Authorized
  if (!isAuthorized) {
    return
  }

  const { baseRegionId } = configuration
  const pathname = router.location.pathname

  if (currentPathname !== pathname || currentBaseRegionId !== baseRegionId) {
    currentPathname = pathname
    currentBaseRegionId = baseRegionId

    const regionId = deriveRegionId(pathname, baseRegionId)
    dispatch(regionActionCreators.setRegion(regionId))
  }
}

export default setRegionEffect