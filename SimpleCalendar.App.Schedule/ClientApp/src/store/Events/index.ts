import { Reducer, Action } from 'redux'
import { Api } from 'src/services/Api'
import { ErrorAction } from '../ErrorAction'
import { ApplicationThunkActionAsync } from 'src/store/ApplicationStore'
import { IEvent } from 'src/services/Api/Models/Event'
import { regionSelectors } from 'src/store/Region'

export interface EventsState {
  isLoading: boolean
  events: IEvent[] | null
  error: any
}

export enum EventsActionTypes {
  FETCH_EVENTS_BEGIN = '[Events] FETCH_EVENTS_BEGIN',
  FETCH_EVENTS_COMPLETE = '[Events] FETCH_EVENTS_COMPLETE',
  FETCH_EVENTS_ERROR = '[Events] FETCH_EVENTS_ERROR',
}

class FetchEventsBegin implements Action {
  readonly type = EventsActionTypes.FETCH_EVENTS_BEGIN
}

class FetchEventsComplete implements Action {
  readonly type = EventsActionTypes.FETCH_EVENTS_COMPLETE
  constructor(public events: any[]) { }
}

class FetchEventsError implements ErrorAction {
  readonly type = EventsActionTypes.FETCH_EVENTS_ERROR
  constructor(public error: any) { }
}

declare type EventsAction =
  FetchEventsBegin |
  FetchEventsComplete |
  FetchEventsError

const DEFAULT_EVENTS_STATE: EventsState = {
  isLoading: false,
  events: null,
  error: null
}

export const eventsReducer: Reducer<EventsState, EventsAction> = (state = DEFAULT_EVENTS_STATE, action) => {
  switch (action.type) {
    case EventsActionTypes.FETCH_EVENTS_BEGIN: return { ...state, isLoading: true }
    case EventsActionTypes.FETCH_EVENTS_COMPLETE: return { ...state, isLoading: false, events: action.events }
    case EventsActionTypes.FETCH_EVENTS_ERROR: return { ...state, isLoading: false, error: action.error }
    default: return state
  }
}

export const eventsActionCreators = {
  fetchEvents: (): ApplicationThunkActionAsync => async (dispatch, getState) => {
    const state = getState()

    if (eventSelectors.hasFetchEventsStarted(state.events)) {
      dispatch(new FetchEventsError('Event fetch has already started'))
      return
    }

    if (!regionSelectors.isFetchRegionCompleted(state.region)) {
      dispatch(new FetchEventsError('Region has not been loaded'))
      return
    }

    dispatch(new FetchEventsBegin())

    const region = regionSelectors.getRegion(state.region)
    try {
      const events = await new Api(state.auth.accessToken).queryEventsToday(region.id, region.timezone)
      dispatch(new FetchEventsComplete(events))
    } catch (e) {
      dispatch(new FetchEventsError(e))
    }
  }
}

export const eventSelectors = {
  hasFetchEventsStarted: (events: EventsState) => events.isLoading || events.events || events.error,
  isFetchEventsCompleted: (events: EventsState) => !!events.events,
  getEvents: (events: EventsState) => {
    if (!events.events) {
      throw new Error('Events not found')
    }
    return events.events
  }
}