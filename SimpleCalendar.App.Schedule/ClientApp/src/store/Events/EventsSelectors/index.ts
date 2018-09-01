import { ApplicationState } from 'src/store/ApplicationState'
import { IEvent } from 'src/services/Api'
import { EventCreationState, EventCollectionState } from 'src/store/Events/EventsState'
import { EventCollectionType } from 'src/store/Events/EventsActions'
import { EventGroupCollection } from './EventGroupCollection'

export * from './EventGroupCollection'

const getEventCollection = (state: ApplicationState, collection: EventCollectionType): EventCollectionState => {
  const { events } = state
  switch (collection) {
    case EventCollectionType.TODAY:
      return events.today
    case EventCollectionType.MY:
      return events.my
    default:
      throw new Error('Event collection type not recognised')
  }
}

export const hasFetchEventsStartedSelector = (state: ApplicationState, collection: EventCollectionType): boolean => {
  const eventCollection = getEventCollection(state, collection)
  return eventCollection.isLoading || eventCollection.events || eventCollection.error
}

export const getEventsSelector = (state: ApplicationState, collection: EventCollectionType): IEvent[] => {
  const eventCollection = getEventCollection(state, collection)
  
  const { events, pendingEvents } = eventCollection
  if (!events) {
    throw new Error('Events not found')
  }

  return [
    ...events,
    ...Object.map(pendingEvents, (item: EventCreationState, trackingId: string) => ({
      ...item.create,
      id: trackingId
    }))
  ].orderBy(e => e.startTime)
}

export const getEventSelector = (state: ApplicationState, collection: EventCollectionType, eventId: string): IEvent => {
  const events = getEventsSelector(state, collection)
    
  const event = events.find(e => e.id === eventId)
  if (!event) {
    throw new Error('Event not found')
  }

  return event
}

export const isFetchEventsCompletedSelector = (state: ApplicationState, collection: EventCollectionType): boolean =>
  !!getEventCollection(state, collection).events

export const getEventGroupsSelector = (state: ApplicationState, collection: EventCollectionType): EventGroupCollection =>
  new EventGroupCollection(getEventsSelector(state, collection))
