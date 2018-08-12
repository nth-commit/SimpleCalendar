import { Action } from 'redux'
import { ErrorAction } from 'src/store'
import { IEventCreate, IEvent } from 'src/services/Api'

export enum EventsActionTypes {
  FETCH_EVENTS_BEGIN = '[Events] FETCH_EVENTS_BEGIN',
  FETCH_EVENTS_COMPLETE = '[Events] FETCH_EVENTS_COMPLETE',
  FETCH_EVENTS_ERROR = '[Events] FETCH_EVENTS_ERROR',
  CREATE_EVENT_BEGIN = '[Events] CREATE_EVENT_BEGIN',
  CREATE_EVENT_COMPLETE = '[Events] CREATE_EVENT_COMPLETE',
  CREATE_EVENT_ERROR = '[Events] CREATE_EVENT_ERROR',
}

export class FetchEventsBegin implements Action {
  readonly type = EventsActionTypes.FETCH_EVENTS_BEGIN
}

export class FetchEventsComplete implements Action {
  readonly type = EventsActionTypes.FETCH_EVENTS_COMPLETE
  constructor(public events: any[]) { }
}

export class FetchEventsError implements ErrorAction {
  readonly type = EventsActionTypes.FETCH_EVENTS_ERROR
  constructor(public error: any) { }
}

export class CreateEventBegin implements Action {
  readonly type = EventsActionTypes.CREATE_EVENT_BEGIN
  constructor(public eventCreate: IEventCreate, public trackingId: number) { }
}

export class CreateEventComplete implements Action {
  readonly type = EventsActionTypes.CREATE_EVENT_COMPLETE
  constructor(public event: IEvent, public trackingId: number) { }
}

export class CreateEventError implements ErrorAction {
  readonly type = EventsActionTypes.CREATE_EVENT_ERROR
  constructor(public error: any, public trackingId: number) { }
}

export type EventsAction =
  FetchEventsBegin |
  FetchEventsComplete |
  FetchEventsError |
  CreateEventBegin |
  CreateEventComplete |
  CreateEventError