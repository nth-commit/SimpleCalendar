import { Reducer } from 'redux'
import { EventsState } from 'src/store/Events/EventsState'
import * as EventsActions from './EventsActions'
const { EventsActionTypes } = EventsActions

const DEFAULT_EVENTS_STATE: EventsState = {
  isLoading: false,
  events: null,
  error: null
}

export const eventsReducer: Reducer<EventsState, EventsActions.EventsAction> = (state = DEFAULT_EVENTS_STATE, action) => {
  switch (action.type) {
    case EventsActionTypes.FETCH_EVENTS_BEGIN: return { ...state, isLoading: true }
    case EventsActionTypes.FETCH_EVENTS_COMPLETE: return { ...state, isLoading: false, events: action.events }
    case EventsActionTypes.FETCH_EVENTS_ERROR: return { ...state, isLoading: false, error: action.error }
    default: return state
  }
}

export * from './EventsSelectors'
export * from './EventsActionCreators'
export * from './EventsState'