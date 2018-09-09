import { Reducer } from 'redux'
import { EventsState, EventCreationsState, EventCollectionState } from 'src/store/Events/EventsState'
import * as EventsActions from './EventsActions'
import { InvokedReducer } from 'src/store/InvokedReducer'
import { IEvent } from 'src/services/Api'
const { EventsActionTypes } = EventsActions

const DEFAULT_EVENT_COLLECTION_STATE: EventCollectionState = {
  isLoading: false,
  events: null,
  pendingEvents: {},
  error: null
}

const DEFAULT_EVENTS_STATE: EventsState = {
  today: DEFAULT_EVENT_COLLECTION_STATE,
  my: DEFAULT_EVENT_COLLECTION_STATE
}

export const eventsReducer: Reducer<EventsState, EventsActions.EventsAction> = (state = DEFAULT_EVENTS_STATE, action) => {
  switch (action.type) {
    case EventsActionTypes.CLEAR_EVENTS: return clearEvents(state, action)
    case EventsActionTypes.FETCH_EVENTS_BEGIN: return updateEventCollection(state, action.collectionType, collection => ({ ...collection, isLoading: true  }))
    case EventsActionTypes.FETCH_EVENTS_COMPLETE: return updateEventCollection(state, action.collectionType, collection => ({ ...collection, isLoading: false, events: action.events }))
    case EventsActionTypes.FETCH_EVENTS_ERROR: return updateEventCollection(state, action.collectionType, collection => ({ ...collection, isLoading: false, error: action.error }))
    case EventsActionTypes.CREATE_EVENT_BEGIN: return createEventBegin(state, action)
    case EventsActionTypes.CREATE_EVENT_COMPLETE: return createEventComplete(state, action)
    default: return state
  }
}

const clearEvents: InvokedReducer<EventsState, EventsActions.ClearEvents> = (state, { collectionType }) =>
  updateEventCollection(state, collectionType, () => DEFAULT_EVENT_COLLECTION_STATE)

const createEventBegin: InvokedReducer<EventsState, EventsActions.CreateEventBegin> = (state, { collectionType, create, trackingId }) =>
  updateEventCollection(state, collectionType, collection => ({
    ...collection,
    pendingEvents: {
      ...collection.pendingEvents,
      [trackingId]: {
        create,
        errors: []
      }
    }
  }))

const createEventComplete: InvokedReducer<EventsState, EventsActions.CreateEventComplete> = (state, { collectionType, event, trackingId }) => 
  updateEventCollection(state, collectionType, collection => ({
    ...collection,
    events: [
      ...(collection.events as IEvent[]),
      event
    ],
    pendingEvents: Object.filter(collection.pendingEvents, t => trackingId.toString() !== t) as EventCreationsState
  }))

const updateEventCollection = (
  state: EventsState,
  collectionType: EventsActions.EventCollectionType,
  updateCollectionFunc: (eventCollection: EventCollectionState) => EventCollectionState): EventsState => ({
    my: collectionType === EventsActions.EventCollectionType.MY ? updateCollectionFunc(state.my) : state.my,
    today: collectionType === EventsActions.EventCollectionType.TODAY ? updateCollectionFunc(state.today) : state.today
  })

export * from './EventsSelectors'
export * from './EventsActionCreators'
export * from './EventsState'
export { EventCollectionType } from './EventsActions'