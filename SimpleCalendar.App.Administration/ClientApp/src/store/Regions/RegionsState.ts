import { IRegion } from 'src/services/Api'

export interface RegionDictionaryEntry {
  region: IRegion | null
  childRegions: IRegion[] | null
  timestamp: number | null
  error?: any
}

export interface RegionDictionary {
  [regionId: string ]: RegionDictionaryEntry | undefined
}

export interface RegionsState {
  regionId: string | null
  regionDictionary: RegionDictionary
  loading: boolean
}