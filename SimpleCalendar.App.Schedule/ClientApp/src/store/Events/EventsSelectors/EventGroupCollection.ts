import * as moment from 'moment'
import { IEvent } from 'src/services/Api'

export enum TimeGrouping {
  Unknown = 0,
  Today,
  Tomorrow,
  ThisWeek,
  NextWeek,
  ThisMonth,
  NextMonth,
  Later,
  Historical
} 

export interface EventGroup {
  timeGrouping: TimeGrouping
  events: IEvent[]
}

export class EventGroupCollection {

  private eventsByTimeGrouping: Map<TimeGrouping, IEvent[]>

  constructor(events: IEvent[]) {
    this.eventsByTimeGrouping = Map.fromArray(eventTimeGroupDefinitions, item => item.timeGrouping, () => [] as IEvent[])

    events.forEach(event => {
      const definition = eventTimeGroupDefinitions.find(g => g.predicate(event))
      if (!definition) {
        return
      }

      const { timeGrouping } = definition
      const eventsForGroup = this.eventsByTimeGrouping.get(timeGrouping)
      if (!eventsForGroup) {
        throw new Error('Unrecognised grouping name')
      }

      this.eventsByTimeGrouping.set(timeGrouping, [...eventsForGroup, event])
    })
  }

  public getGroups(): EventGroup[] {
    return this.getTimeGroupings().map(g => ({
      timeGrouping: g,
      events: this.getEvents(g)
    }))
  }

  public hasGroup(timeGrouping: TimeGrouping): boolean {
    return !!this.getEvents(timeGrouping).length
  }

  public getEvents(timeGrouping: TimeGrouping): IEvent[] {
    const events = this.eventsByTimeGrouping.get(timeGrouping)
    if (!events) {
      throw new Error('Unrecognised TimeGrouping')
    }
    return events
  }

  public getTimeGroupings(): TimeGrouping[] {
    return Array
      .from(this.eventsByTimeGrouping.entries())
      .filter(([g, x]) => x.length)
      .map(([g]) => g)
  }
}

declare interface EventGroupingDefinition {
  timeGrouping: TimeGrouping
  predicate: (event: (IEvent)) => boolean
}

const eventTimeGroupDefinitions: EventGroupingDefinition[] = [
  {
    timeGrouping: TimeGrouping.Today,
    predicate: event =>
      moment().isSame(event.startTime, 'days') ||
      (moment().isAfter(event.startTime) && moment().isBefore(event.endTime))
  },
  {
    timeGrouping: TimeGrouping.Tomorrow,
    predicate: event => moment().diff(event.startTime, 'days') === -1
  },
  {
    timeGrouping: TimeGrouping.ThisWeek,
    predicate: event => moment().isSame(event.startTime, 'weeks')
  },
  {
    timeGrouping: TimeGrouping.NextWeek,
    predicate: event => moment().diff(event.startTime, 'weeks') === -1
  },
  {
    timeGrouping: TimeGrouping.ThisMonth,
    predicate: event => moment().isSame(event.startTime, 'month')
  },
  {
    timeGrouping: TimeGrouping.NextMonth,
    predicate: event => moment().diff(event.startTime, 'month') === -1
  },
  {
    timeGrouping: TimeGrouping.Later,
    predicate: event => moment().diff(event.endTime) < 0
  },
  {
    timeGrouping: TimeGrouping.Historical,
    predicate: () => true
  }
]