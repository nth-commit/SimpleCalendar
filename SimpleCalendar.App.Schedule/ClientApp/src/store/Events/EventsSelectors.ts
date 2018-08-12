import { ApplicationState } from 'src/store/ApplicationState'
import { IEvent, IEventCreate } from 'src/services/Api'
import { PendingEventState } from 'src/store/Events/EventsState'

declare interface EventGroupingDefinition {
  name: string
  predicate: (event: (IEvent | IEventCreate)) => boolean
}

const doesEventSpanDates = (event: IEvent | IEventCreate, start: Date, end: Date): boolean => {
  const eventStart = event.startTime
  const eventEnd = event.endTime

  if (eventStart <= start && start <= eventEnd) { return true }
  if (eventStart <= end && end <= eventEnd) { return true }
  if (start < eventStart && eventEnd < end) { return true }
  return false
}

const eventGroupingDefinitions: EventGroupingDefinition[] = [
  {
    name: 'Today',
    predicate: event => {
      const startOfToday = new Date()
      startOfToday.setHours(0, 0, 0, 0)

      const endOfToday = new Date()
      endOfToday.setHours(23, 59, 59, 0)

      return doesEventSpanDates(event, startOfToday, endOfToday)
    }
  },
  {
    name: 'Tomorrow',
    predicate: event => {
      const startOfTomorrow = new Date()
      startOfTomorrow.setHours(24, 0, 0, 0)

      const endOfTomorrow = new Date()
      endOfTomorrow.setHours(24 + 23, 59, 59, 0)

      return doesEventSpanDates(event, startOfTomorrow, endOfTomorrow)
    }
  },
  {
    name: 'This Week',
    predicate: event => {
      return false
    }
  },
  {
    name: 'Next Week',
    predicate: event => {
      return false
    }
  },
  {
    name: 'This Month',
    predicate: event => {
      return false
    }
  },
  {
    name: 'Next Month',
    predicate: event => {
      return false
    }
  },
  {
    name: 'Later',
    predicate: () => true
  }
]

export interface EventGroup {
  name: string
  events: Array<IEvent | IEventCreate>
}

const getEvents = ({ events }: ApplicationState): Array<IEvent | IEventCreate> => {
  if (!events.events) {
    throw new Error('Events not found')
  }

  return [
    ...events.events,
    ...Object.map(events.pendingEvents, (item: PendingEventState) => item.create)
  ].orderBy(e => e.startTime)
}

export const eventSelectors = {
  hasFetchEventsStarted: ({ events }: ApplicationState): boolean =>
    events.isLoading || events.events || events.error,

  isFetchEventsCompleted: ({ events }: ApplicationState): boolean =>
    !!events.events,

  getEvents,

  getEventGroups: (state: ApplicationState): EventGroup[] => {
    const map = Map.fromArray(eventGroupingDefinitions, item => item.name, () => [] as Array<IEvent | IEventCreate>)

    getEvents(state).forEach(event => {
      const grouping = eventGroupingDefinitions.find(g => g.predicate(event))
      if (!grouping) {
        return
      }

      const { name } = grouping
      const events = map.get(name)
      if (!events) {
        throw new Error('Unrecognised grouping name')
      }

      map.set(name, [...events, event])
    })

    return eventGroupingDefinitions
      .map(g => ({
        name: g.name,
        events: map.get(g.name) as Array<IEvent | IEventCreate>
      } as EventGroup))
      .filter(g => g.events.length)
  }
}