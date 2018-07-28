import { ROOT_REGION_ID } from 'src/constants'
import { IRegion } from 'src/services/Api'

export interface RegionHrefResolver {
  resolve(region?: IRegion): string
}

export class RegionNotOfBaseError {
  public message = 'Region is not related to baseRegionId'
}

const getRegionPath = (regionId: string, baseRegionId: string): string => {
  if (baseRegionId === ROOT_REGION_ID) {
    if (regionId === ROOT_REGION_ID) {
      return '/'
    } else {
      return '/' + regionId
    }
  }

  if (!regionId.startsWith(baseRegionId)) {
    throw new RegionNotOfBaseError()
  }

  return '/' + regionId.substring(baseRegionId.length + 1)
}

export default function createRegionHrefResolver(baseRegionId: string): RegionHrefResolver {
  return {
    resolve: (region?: IRegion): string => region ? getRegionPath(region.id, baseRegionId) : '/'
  }
}
