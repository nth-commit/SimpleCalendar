import { IEvent, IEventCreate } from 'src/services/Api'

export interface PendingEventState {
  create: IEventCreate
  errors: any[]
}

export interface PendingEventsState {
  [trackingId: string]: PendingEventState
}

export interface EventsState {
  isLoading: boolean
  events: IEvent[] | null
  pendingEvents: PendingEventsState
  error: any
}