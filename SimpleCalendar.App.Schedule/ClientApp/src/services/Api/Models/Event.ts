export interface IBaseEvent {
  id: string
  regionId: string
  name: string
  description: string
}

export interface IEvent extends IBaseEvent {
  startTime: Date
  endTime: Date
}

export interface IEventResponse extends IBaseEvent {
  startTime: string
  endTime: string
}

export interface IEventCreate {
  regionId: string
  startTime: Date
  endTime: Date
  name: string
  description: string
}
