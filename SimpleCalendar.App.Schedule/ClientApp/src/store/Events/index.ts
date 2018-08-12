import { Reducer } from 'redux'
import { EventsState, PendingEventsState } from 'src/store/Events/EventsState'
import * as EventsActions from './EventsActions'
import { InvokedReducer } from 'src/store/InvokedReducer'
import { IEvent } from 'src/services/Api'
const { EventsActionTypes } = EventsActions

const DEFAULT_EVENTS_STATE: EventsState = {
  isLoading: false,
  events: null,
  pendingEvents: {},
  error: null
}

export const eventsReducer: Reducer<EventsState, EventsActions.EventsAction> = (state = DEFAULT_EVENTS_STATE, action) => {
  switch (action.type) {
    case EventsActionTypes.FETCH_EVENTS_BEGIN: return { ...state, isLoading: true }
    case EventsActionTypes.FETCH_EVENTS_COMPLETE: return { ...state, isLoading: false, events: action.events }
    case EventsActionTypes.FETCH_EVENTS_ERROR: return { ...state, isLoading: false, error: action.error }
    case EventsActionTypes.CREATE_EVENT_BEGIN: return createEventBegin(state, action)
    case EventsActionTypes.CREATE_EVENT_COMPLETE: return createEventComplete(state, action)
    default: return state
  }
}

const createEventBegin: InvokedReducer<EventsState, EventsActions.CreateEventBegin> = (state, { create, trackingId }) => ({
  ...state,
  pendingEvents: {
    ...state.pendingEvents,
    [trackingId]: {
      create,
      errors: []
    }
  }
})

const createEventComplete: InvokedReducer<EventsState, EventsActions.CreateEventComplete> = (state, { event, trackingId }) => ({
  ...state,
  events: [
    ...(state.events as IEvent[]),
    event
  ],
  pendingEvents: Object.filter(state.pendingEvents, t => trackingId.toString() !== t) as PendingEventsState
})

export * from './EventsSelectors'
export * from './EventsActionCreators'
export * from './EventsState'