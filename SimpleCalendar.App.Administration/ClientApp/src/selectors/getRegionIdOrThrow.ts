import { ApplicationState } from 'src/store'

export class RegionNotSetError { }

export default function getRegionIdOrThrow(state: ApplicationState): string {
  const { regions } = state

  const { regionId } = regions
  if (!regionId) {
    throw new RegionNotSetError()
  }

  return regionId
}