import { ApplicationState } from 'src/store/ApplicationState'
import { IEvent, IEventCreate } from 'src/services/Api'
import { PendingEventState } from 'src/store/Events/EventsState'

export const eventSelectors = {
  hasFetchEventsStarted: ({ events }: ApplicationState): boolean =>
    events.isLoading || events.events || events.error,

  isFetchEventsCompleted: ({ events }: ApplicationState): boolean =>
    !!events.events,

  getEvents: ({ events }: ApplicationState): Array<IEvent | IEventCreate> => {
    if (!events.events) {
      throw new Error('Events not found')
    }

    return [
      ...events.events,
      ...Object.map(events.pendingEvents, (item: PendingEventState) => item.create)
    ].orderBy(e => e.startTime)
  }
}