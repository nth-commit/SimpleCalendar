import { ApplicationState } from './ApplicationState'
import { ROOT_REGION_ID } from 'src/constants'

export function getRegionId(state: ApplicationState) {
  const path = state.router.location.pathname
  const baseRegionId = state.configuration.baseRegionId
  
  const requestedRegionId = path.substring(1)
  if (requestedRegionId) {
    return getAbsoluteRegionId(requestedRegionId, baseRegionId)
  } else {
    return baseRegionId
  }
}

function getAbsoluteRegionId(relativeRegionId: string, baseRegionId: string): string {
  if (baseRegionId === ROOT_REGION_ID) {
    return relativeRegionId
  }
  return `${baseRegionId}/${relativeRegionId}`
}
