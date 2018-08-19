import { ApplicationState } from 'src/store/ApplicationState'
import { IEvent } from 'src/services/Api'
import { PendingEventState } from 'src/store/Events/EventsState'
import { EventGroupCollection } from './EventGroupCollection'

export * from './EventGroupCollection'

const getEvents = ({ events }: ApplicationState): IEvent[] => {
  if (!events.events) {
    throw new Error('Events not found')
  }

  return [
    ...events.events,
    ...Object.map(events.pendingEvents, (item: PendingEventState, trackingId: string) => ({
      ...item.create,
      id: trackingId
    }))
  ].orderBy(e => e.startTime)
}

export const eventSelectors = {
  hasFetchEventsStartedSelector: ({ events }: ApplicationState): boolean =>
    events.isLoading || events.events || events.error,

  isFetchEventsCompletedSelector: ({ events }: ApplicationState): boolean =>
    !!events.events,

  getEventsSelector: getEvents,

  getEventGroupsSelector: (state: ApplicationState): EventGroupCollection =>
    new EventGroupCollection(getEvents(state)),

  getEventSelector: (state: ApplicationState, eventId: string): IEvent => {
    const events = getEvents(state)
    
    const event = events.find(e => e.id === eventId)
    if (!event) {
      throw new Error('Event not found')
    }

    return event
  }
}
