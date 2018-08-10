import { Reducer, Action } from 'redux'
import { IRegion, Api } from 'src/services/Api'
import { ErrorAction } from '../ErrorAction'
import { ApplicationThunkActionAsync } from 'src/store/ApplicationStore'

export interface RegionState {
  isLoading: boolean
  region: IRegion | null
  error: any
}

export enum RegionActionTypes {
  FETCH_REGION_BEGIN = '[Region] FETCH_REGION_BEGIN',
  FETCH_REGION_COMPLETE = '[Region] FETCH_REGION_COMPLETE',
  FETCH_REGION_ERROR = '[Region] FETCH_REGION_ERROR',
}

class FetchRegionBegin implements Action {
  readonly type = RegionActionTypes.FETCH_REGION_BEGIN
}

class FetchRegionComplete implements Action {
  readonly type = RegionActionTypes.FETCH_REGION_COMPLETE
  constructor(public region: IRegion) { }
}

class FetchRegionError implements ErrorAction {
  readonly type = RegionActionTypes.FETCH_REGION_ERROR
  constructor(public error: any) { }
}

declare type RegionAction =
  FetchRegionBegin |
  FetchRegionComplete |
  FetchRegionError

const DEFAULT_REGION_STATE: RegionState = {
  isLoading: false,
  region: null,
  error: null
}

export const regionReducer: Reducer<RegionState, RegionAction> = (state = DEFAULT_REGION_STATE, action) => {
  switch (action.type) {
    case RegionActionTypes.FETCH_REGION_BEGIN: return { ...state, isLoading: true }
    case RegionActionTypes.FETCH_REGION_COMPLETE: return { ...state, isLoading: false, region: action.region }
    case RegionActionTypes.FETCH_REGION_ERROR: return { ...state, isLoading: false, error: action.error }
    default: return state
  }
}

export const regionActionCreators = {
  fetchRegion: (): ApplicationThunkActionAsync => async (dispatch, getState) => {
    const { auth, configuration } = getState()

    dispatch(new FetchRegionBegin())

    try {
      const region = await new Api(auth.accessToken).getRegion(configuration.regionId)
      dispatch(new FetchRegionComplete(region))
    } catch (e) {
      dispatch(new FetchRegionError(e))
    }
  }
}

export const regionSelectors = {
  hasFetchRegionStarted: (region: RegionState) => region.isLoading || region.region || region.error,
  isFetchRegionCompleted: (region: RegionState) => !!region.region,
  getRegion: (region: RegionState) => {
    if (!region.region) {
      throw new Error('Region not found')
    }
    return region.region
  }
}