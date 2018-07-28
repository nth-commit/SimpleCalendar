import { Reducer } from 'redux'
import * as Actions from './Actions'
import { enumerateRegionId } from './Utility'
import actionCreators from './ActionCreators'
import { InvokedReducer } from 'src/store/InvokedReducer'
import { RegionsState, RegionDictionary, RegionDictionaryEntry } from 'src/store/Regions/RegionsState'

export class RegionNotFoundError {
  constructor(public regionId: string) { }
}

const DEFAULT_STATE: RegionsState = {
  regionId: null,
  regionDictionary: {},
  loading: false
}

const updateRegions = (state: RegionsState, newRegions: RegionDictionary): RegionsState =>
  ({ ...state, ...{ regionDictionary: newRegions } })

const updateRegionInRegions = (state: RegionsState, regionId: string, newPartialRegion: Partial<RegionDictionaryEntry>): RegionsState => {
  const { regionDictionary } = state

  const region = regionDictionary[regionId]
  if (!region) {
    throw new RegionNotFoundError(regionId)
  }

  return updateRegions(state, {
    ...state.regionDictionary,
    [regionId]: {
      ...region,
      ...newPartialRegion
    }
  })
}

const fetchRegionBegin: InvokedReducer<RegionsState, Actions.FetchRegionBegin> = (state, { regionId, timestamp }) =>
  updateRegions(state, {
    ...state.regionDictionary,
    [regionId]: {
      region: null,
      childRegions: null,
      timestamp
    }
  })

const fetchRegionComplete: InvokedReducer<RegionsState, Actions.FetchRegionComplete> = (state, { region, childRegions }) =>
  updateRegionInRegions(state, region.id, { region, childRegions })

const fetchRegionError: InvokedReducer<RegionsState, Actions.FetchRegionError> = (state, { regionId, error }) =>
  updateRegionInRegions(state, regionId, { error })

const setRegionBegin: InvokedReducer<RegionsState, Actions.SetRegionBegin> = (state, { regionId }) =>
  ({ ...state, regionId, loading: true })

const setRegionComplete: InvokedReducer<RegionsState, Actions.SetRegionComplete> = (state, { }) => {
  const { regionId } = state
  if (regionId) {
    enumerateRegionId(regionId)
      .filter(r => !state.regionDictionary[r])
      .forEach(r => { throw new RegionNotFoundError(r) })
  }
  return { ...state, loading: false }
}

export const regionsNewReducer: Reducer = (state: RegionsState = DEFAULT_STATE, action: Actions.RegionAction) => {
  switch (action.type) {
    case Actions.RegionsActionTypes.FETCH_REGION_BEGIN: return fetchRegionBegin(state, action)
    case Actions.RegionsActionTypes.FETCH_REGION_COMPLETE: return fetchRegionComplete(state, action)
    case Actions.RegionsActionTypes.FETCH_REGION_ERROR: return fetchRegionError(state, action)
    case Actions.RegionsActionTypes.SET_REGION_BEGIN: return setRegionBegin(state, action)
    case Actions.RegionsActionTypes.SET_REGION_COMPLETE: return setRegionComplete(state, action)
    default: return state
  }
}

export const regionActionCreators = actionCreators