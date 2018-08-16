import * as moment from 'moment'
import { ApplicationState } from 'src/store/ApplicationState'
import { IEvent } from 'src/services/Api'
import { PendingEventState } from 'src/store/Events/EventsState'

export enum TimeGrouping {
  Unknown = 0,
  Today,
  Tomorrow,
  ThisWeek,
  NextWeek,
  ThisMonth,
  NextMonth,
  Later
} 

declare interface EventGroupingDefinition {
  timeGrouping: TimeGrouping 
  name: string
  predicate: (event: (IEvent)) => boolean
}

const eventGroupingDefinitions: EventGroupingDefinition[] = [
  {
    timeGrouping: TimeGrouping.Today,
    name: 'Today',
    predicate: event => moment().isSame(event.startTime, 'days')
  },
  {
    timeGrouping: TimeGrouping.Tomorrow,
    name: 'Tomorrow',
    predicate: event => moment().diff(event.startTime, 'days') === 1
  },
  {
    timeGrouping: TimeGrouping.ThisWeek,
    name: 'This Week',
    predicate: event => moment().isSame(event.startTime, 'weeks')
  },
  {
    timeGrouping: TimeGrouping.NextWeek,
    name: 'Next Week',
    predicate: event => moment().diff(event.startTime, 'weeks') === 1
  },
  {
    timeGrouping: TimeGrouping.ThisMonth,
    name: 'This Month',
    predicate: event => moment().isSame(event.startTime, 'month')
  },
  {
    timeGrouping: TimeGrouping.NextMonth,
    name: 'Next Month',
    predicate: event => moment().diff(event.startTime, 'month') === 1
  },
  {
    timeGrouping: TimeGrouping.Later,
    name: 'Later',
    predicate: () => true
  }
]

export interface EventGroup {
  timeGrouping: TimeGrouping
  name: string
  events: IEvent[]
}

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
  hasFetchEventsStarted: ({ events }: ApplicationState): boolean =>
    events.isLoading || events.events || events.error,

  isFetchEventsCompleted: ({ events }: ApplicationState): boolean =>
    !!events.events,

  getEvents,

  getEventGroups: (state: ApplicationState): EventGroup[] => {
    const map = Map.fromArray(eventGroupingDefinitions, item => item.name, () => [] as IEvent[])

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
        timeGrouping: g.timeGrouping,
        name: g.name,
        events: map.get(g.name) as IEvent[]
      } as EventGroup))
      .filter(g => g.events.length)
  }
}