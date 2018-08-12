import { EventsState } from 'src/store/Events/EventsState'

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