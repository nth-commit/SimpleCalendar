import { IEvent, IEventCreate } from 'src/services/Api'

export interface EventCreationState {
  create: IEventCreate
  errors: any[]
}

export interface EventCreationsState {
  [trackingId: string]: EventCreationState
}

export interface EventCollectionState {
  isLoading: boolean
  events: IEvent[] | null
  pendingEvents: EventCreationsState
  error: any
}

export interface EventsState {
  today: EventCollectionState
  my: EventCollectionState
}
